using com.tweetapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.DAL
{
    public interface IUserRepository
    {
        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>True, if user registered successfully, False otherwise</returns>
        public Task<bool> Register(User user);

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="password">Password</param>
        /// <returns>Token in string format</returns>
        public Task<string> Login(string username, string password);

        /// <summary>
        /// Get logged in user
        /// </summary>
        /// <returns>User</returns>
        public Task<User> GetLoggedInUser();

        /// <summary>
        /// Forgot password
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New password</param>
        /// <returns>True if password changed successfully, False otherwise</returns>
        public Task<bool> ForgotPassword(string username, string oldPassword, string newPassword);

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>Users</returns>
        public Task<IEnumerable<User>> GetAllUsers();

        /// <summary>
        /// Search user
        /// </summary>
        /// <param name="username">User name</param>
        /// <returns>User</returns>
        public Task<User> SearchUser(string username);

        /// <summary>
        /// Is email id already taken
        /// </summary>
        /// <param name="username">User name</param>
        /// <returns>True, if email id already taken by an existing user, False otherwise</returns>
        public Task<bool?> IsEmailIdAlreadyTaken(string emailId);
    }
}
