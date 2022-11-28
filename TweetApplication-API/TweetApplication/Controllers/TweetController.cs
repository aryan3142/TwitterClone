using com.tweetapp.Models;
using com.tweetapp.Models.Dtos;
using com.tweetapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.Controllers
{
    [Route("api/v1.0/tweets")]
    [ApiController]
    [Authorize]
    public class TweetController : ControllerBase
    {
        /// <summary>
        /// Tweet service
        /// </summary>
        private readonly ITweetService tweetService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TweetController"/> class.
        /// </summary>
        /// <param name="tweetService">Tweet service</param>
        public TweetController(ITweetService tweetService)
        {
            this.tweetService = tweetService;
        }

        /// <summary>
        /// Get all tweets
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllTweets")]
        public async Task<IActionResult> GetAllTweets()
        {
            IEnumerable<Tweet> tweetList = await tweetService.GetAllTweets();
            if(tweetList != null)
            {
                return this.Ok(tweetList);
            }

            return this.NoContent();
        }

        /// <summary>
        /// Get user tweets
        /// </summary>
        /// <param name="username">User name</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{username}/GetUserTweet")]
        public async Task<IActionResult> GetUserTweets(string username)
        {
            IEnumerable<Tweet> userTweetList = await tweetService.GetUserTweets(username);
            if(userTweetList != null)
            {
                return Ok(userTweetList);
            }

            return NoContent();
        }

        /// <summary>
        /// Add new tweet
        /// </summary>
        /// <param name="tweet">Tweet</param>
        /// <param name="username">User name</param>
        /// <returns>Tweet</returns>
        [HttpPost]
        [Route("{username}/Add")]
        public async Task<IActionResult> AddNewTweet(PostNewTweetDto tweet, string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Tweet res = await tweetService.AddNewTweet(tweet, username);
            if(res != null)
            {
                return Ok("New tweet posted!");
            }

            return BadRequest(); 
        }

        /// <summary>
        /// Update tweet
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="id">Id</param>
        /// <param name="newMessage">New message</param>
        /// <returns>Updated tweet</returns>
        [HttpPut]
        [Route("{username}/UpdateTweet/{id}")]
        public async Task<IActionResult> UpdateTweet(string username, string id, [FromBody] string newMessage)
        {
            Tweet updatedTweet = await tweetService.UpdateTweet(username, id, newMessage);
            if(updatedTweet != null)
            {
                return Ok(updatedTweet);
            }

            return BadRequest();
        }

        /// <summary>
        /// Delete tweet
        /// </summary>
        /// <param name="username">User name</param>
        /// <returns>True, if tweet is deleted successfully, False otherwise</returns>
        [HttpDelete]
        [Route("{username}/delete/{id}")]
        public async Task<IActionResult> DeleteTweet(string username, string id)
        {
            bool isTweetDeleted = await tweetService.DeleteTweet(username, id);
            if(isTweetDeleted)
            {
                return Ok("Tweet deleted sucessfully!");
            }

            return BadRequest();
        }

        /// <summary>
        /// Like or unlike tweet
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="id">Id</param>
        /// <returns>Integer value representing number of likes on the tweet</returns>
        [HttpPut]
        [Route("{username}/like/{id}")]
        public async Task<IActionResult> LikeUnlikeTweet(string username, string id)
        {
            int numberOfLikeOnTweet = await tweetService.LikeUnlikeTweet(username, id);          
            if(numberOfLikeOnTweet >= 0)
            {
                return Ok(numberOfLikeOnTweet);
            }

            return BadRequest();
        }

        /// <summary>
        /// Reply tweet
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="id">Id</param>
        /// <param name="message">Message</param>
        /// <returns>True if reply to tweet saved successfully, False otherwise</returns>
        [HttpPost]
        [Route("{username}/reply/{id}")]
        public async Task<IActionResult> Reply(string username, string id, [FromBody] string message)
        {
            if(string.IsNullOrWhiteSpace(message))
            {
                return BadRequest();
            }
            
            bool response = await tweetService.Reply(username, id, message);
            if (response == true)
                return Ok(response);
            else
                return BadRequest("Not able to reply to tweet!");
        }
    }
}
