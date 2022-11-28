using com.tweetapp.Models;
using com.tweetapp.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.Services
{
    public interface IUserService
    {
        public Task<bool> Register(User user);

        public Task<string> Login(string username, string password);

        public Task<ViewUserDto> GetLoggedInUser();

        public Task<bool> ForgotPassword(string username, string oldPassword, string newPassword);

        public Task<IEnumerable<ViewUserDto>> GetAllUsers();

        public Task<ViewUserDto> SearchUser(string username);

        public Task<bool?> IsEmailIdAlreadyTaken(string emailId);
    }
}
