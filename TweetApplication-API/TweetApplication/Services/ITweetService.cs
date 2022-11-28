using com.tweetapp.Models;
using com.tweetapp.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.Services
{
    public interface ITweetService
    {
        /// <summary>
        /// Get all tweets
        /// </summary>
        /// <returns>Tweets</returns>
        public Task<IEnumerable<Tweet>> GetAllTweets();

        /// <summary>
        /// Get user tweets
        /// </summary>
        /// <param name="username">User name</param>
        /// <returns>Tweets</returns>
        public Task<IEnumerable<Tweet>> GetUserTweets(string username);

        /// <summary>
        /// Add new tweet
        /// </summary>
        /// <param name="tweet">Tweet</param>
        /// <returns>Tweet</returns>
        public Task<Tweet> AddNewTweet(PostNewTweetDto tweet, string username);

        /// <summary>
        /// Update user tweet
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="id">Id</param>
        /// <param name="newMessage">New message</param>
        /// <returns>Tweet</returns>
        public Task<Tweet> UpdateTweet(string username, string id, string message);

        /// <summary>
        /// Delete tweet
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="id">Id</param>
        /// <returns>True, if tweet is deleted successfully, False otherwise</returns>
        public Task<bool> DeleteTweet(string username, string id);

        /// <summary>
        /// Like tweet
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="id">Id</param>
        /// <returns>Integer value representing number of likes on the tweet</returns>
        public Task<int> LikeUnlikeTweet(string username, string id);

        /// <summary>
        /// Reply tweet
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="replyTweet">Reply tweet</param>
        /// <returns>True if reply to tweet saved successfully, False otherwise</returns>
        public Task<bool> Reply(string username, string id, string message);
    }
}
