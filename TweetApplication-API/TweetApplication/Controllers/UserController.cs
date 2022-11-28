using com.tweetapp.Models;
using com.tweetapp.Models.Dtos;
using com.tweetapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace com.tweetapp.Controllers
{
    [Authorize]
    [Route("api/v1.0/tweets")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService _userService)
        {
            userService = _userService;
        }

        //POST /api/v1.0/tweets/register Register as new user
        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool? res = await userService.IsEmailIdAlreadyTaken(user.EmailId);

            if(res != null && res != false)
            {
                return BadRequest($"User with {user.EmailId} already registered!");
            }
            
            var response = await userService.Register(user);
            if(response == false)
            {
                return BadRequest("Error occurred while registering user!");
            }

            return Ok("Registered Successfully!");
        }

        //GET /api/v1.0/tweets/login Login
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var token = await userService.Login(username, password);

            if (token != null)
                return Ok(new { token, username });
            else
                return Unauthorized("You are not registered user!");
        }

        [HttpGet]
        [Authorize]
        [Route("getLoggedInUser")]
        //GET : /api/UserProfile
        public async Task<IActionResult> GetLoggedInUser()
        {
            var user = await userService.GetLoggedInUser();

            if(user != null)
            {
                return Ok(user);
                    //new
                    //{
                    //    user.EmailId,
                    //    user.FirstName,
                    //    user.LastName,
                    //    user.DateOfBirth
                    //});
            }

            return BadRequest("No logged in user!");
        }

        //GET /api/v1.0/tweets/<username>/forgot Forgot password
        [HttpPut]
        [Route("{username}/forgot")]
        public async Task<IActionResult> ForgotPassword(string username, string oldPassword, string newPassword)
        {
            var result = await userService.ForgotPassword(username, oldPassword, newPassword);

            if(result == true)
            {
                return Ok("Password changed successfully!");
            }

            return BadRequest("Cannot change password!");
        }

        [HttpGet]
        [Route("users/all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var userList = await userService.GetAllUsers();

            return Ok(userList);
        }

        //GET /api/v/1.0/tweets/user/search/username* Search by username
        [HttpGet]
        [Route("user/search/{username}")]
        public async Task<IActionResult> SearchUser(string username)
        {
            ViewUserDto user = await userService.SearchUser(username);

            if(user != null)
            {
                return Ok(user);
            }
            return BadRequest($"User {username} not found!");
        }

    }
}
