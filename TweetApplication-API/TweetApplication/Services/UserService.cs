using AutoMapper;
using com.tweetapp.DAL;
using com.tweetapp.Models;
using com.tweetapp.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userDAL;

        private readonly IMapper mapper;

        public UserService(IUserRepository _userDAL, IMapper _mapper)
        {
            userDAL = _userDAL;
            mapper = _mapper;
        }

        public async Task<bool> Register(User user)
        {
            try
            {
                bool respone = await userDAL.Register(user);
                return respone;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<string> Login(string username, string password)
        {
            var token = await userDAL.Login(username, password);
            if (token != null)
            {
                return token;
            }
            return null;
        }

        public async Task<ViewUserDto> GetLoggedInUser()
        {
            User user = await userDAL.GetLoggedInUser();
            return mapper.Map<ViewUserDto>(user);
        }

        public async Task<bool> ForgotPassword(string username, string oldPassword, string newPassword)
        {
            var result = await userDAL.ForgotPassword(username, oldPassword, newPassword);
            return result;
        }

        public async Task<IEnumerable<ViewUserDto>> GetAllUsers()
        {
            try
            {
                var users = await userDAL.GetAllUsers();
                var userDetail = new List<ViewUserDto>();
                foreach (var user in users)
                {
                    userDetail.Add(mapper.Map<ViewUserDto>(user));
                }
                return userDetail;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ViewUserDto> SearchUser(string username)
        {
            try
            {
                var user = await userDAL.SearchUser(username);
                return mapper.Map<ViewUserDto>(user);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool?> IsEmailIdAlreadyTaken(string emailId)
        {
            try
            {
                return await userDAL.IsEmailIdAlreadyTaken(emailId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
