using com.tweetapp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace com.tweetapp.DAL
{
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// Configuration interface
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Http context accessor interface
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Secret key
        /// </summary>
        public readonly string key;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="configuration">Configuration</param>
        /// <param name="httpContextAccessor">HttpContextAccessor</param>
        public UserRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
            key = configuration.GetSection("ApplicationSettings:JWT_Secret").Value;
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>True, if user registered successfully, False otherwise</returns>
        public async Task<bool> Register(User user)
        {
            MongoClient dbClient = new MongoClient(configuration.GetConnectionString("TweetAppCon"));
            await dbClient.GetDatabase("TweetAppDb").GetCollection<User>("User").InsertOneAsync(user);
            return true;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="password">Password</param>
        /// <returns>Token in string format</returns>
        public async Task<string> Login(string username, string password)
        {
            MongoClient dbClient = new MongoClient(configuration.GetConnectionString("TweetAppCon"));
            User user = await dbClient.GetDatabase("TweetAppDb").GetCollection<User>("User").Find($"{{ emailId : '{username}' , password : '{password}' }}").FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("Username", user.EmailId)
                    }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            String token = tokenHandler.WriteToken(securityToken);
            return token;
        }

        /// <summary>
        /// Get logged in user
        /// </summary>
        /// <returns>User</returns>
        public async Task<User> GetLoggedInUser()
        {
            //First get user claims    
            var claims = httpContextAccessor.HttpContext.User.Identities.First().Claims.ToList();

            //Filter specific claim    
            String username = claims?.FirstOrDefault(x => x.Type.Equals("Username"))?.Value;
            MongoClient dbClient = new MongoClient(configuration.GetConnectionString("TweetAppCon"));
            User user = await dbClient.GetDatabase("TweetAppDb").GetCollection<User>("User").Find(u => u.EmailId == username).FirstOrDefaultAsync();
            return user;
        }

        /// <summary>
        /// Forgot password
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New password</param>
        /// <returns>True if password changed successfully, False otherwise</returns>
        public async Task<bool> ForgotPassword(string username, string oldPassword, string newPassword)
        {
            MongoClient dbClient = new MongoClient(configuration.GetConnectionString("TweetAppCon"));
            User user = await dbClient.GetDatabase("TweetAppDb").GetCollection<User>("User").Find($"{{ emailId : '{username}' }}").FirstOrDefaultAsync();
            if(user.Password == oldPassword)
            {
                var filter = Builders<User>.Filter.Eq("EmailId", username);
                var update = Builders<User>.Update.Set("Password", newPassword);
                UpdateResult res = await dbClient.GetDatabase("TweetAppDb").GetCollection<User>("User").UpdateOneAsync(filter, update);
                return res.ModifiedCount > 0;
            }

            return false;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>Users</returns>
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            MongoClient dbClient = new MongoClient(configuration.GetConnectionString("TweetAppCon"));
            var dbList = await dbClient.GetDatabase("TweetAppDb").GetCollection<User>("User").Find(u => true).ToListAsync();
            return dbList;
        }

        /// <summary>
        /// Search user
        /// </summary>
        /// <param name="username">User name</param>
        /// <returns>User</returns>
        public async Task<User> SearchUser(string username)
        {
            MongoClient dbClient = new MongoClient(configuration.GetConnectionString("TweetAppCon"));
            var filter = Builders<User>.Filter.Eq("emailId", username);
            var user = await dbClient.GetDatabase("TweetAppDb").GetCollection<User>("User").Find(filter).FirstOrDefaultAsync();
            return user;
        }

        /// <summary>
        /// Is email id already taken
        /// </summary>
        /// <param name="username">User name</param>
        /// <returns>True, if email id already taken by an existing user, False otherwise</returns>
        public async Task<bool?> IsEmailIdAlreadyTaken(string username)
        {
            MongoClient dbClient = new MongoClient(configuration.GetConnectionString("TweetAppCon"));
            var filter = Builders<User>.Filter.Eq("EmailId", username);
            User user = await dbClient.GetDatabase("TweetAppDb").GetCollection<User>("User").Find(filter).FirstOrDefaultAsync();
            if (user != null)
                return true;

            return false;
        }
    }

    public static class ClaimsPrincipalExtensions
    {
        public static T GetLoggedInUserId<T>(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var loggedInUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(loggedInUserId, typeof(T));
            }
            else if (typeof(T) == typeof(int) || typeof(T) == typeof(long))
            {
                return loggedInUserId != null ? (T)Convert.ChangeType(loggedInUserId, typeof(T)) : (T)Convert.ChangeType(0, typeof(T));
            }
            else
            {
                throw new Exception("Invalid type provided");
            }
        }

        /// <summary>
        /// Get logged in user name
        /// </summary>
        /// <param name="principal">ClaimsPrincipal</param>
        /// <returns>Logged in user name</returns>
        public static string GetLoggedInUserName(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirstValue(ClaimTypes.Name);
        }


        /// <summary>
        /// Get logged in user email
        /// </summary>
        /// <param name="principal">ClaimsPrincipal</param>
        /// <returns>Logged in user email</returns>
        public static string GetLoggedInUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirstValue(ClaimTypes.Email);
        }
    }
}
