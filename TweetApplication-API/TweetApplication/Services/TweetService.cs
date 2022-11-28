using AutoMapper;
using com.tweetapp.DAL;
using com.tweetapp.Models;
using com.tweetapp.Models.Dtos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.Services
{
    public class TweetService: ITweetService
    {
        /// <summary>
        /// Tweet repository
        /// </summary>
        private readonly ITweetRepository tweetRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Mapper class
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TweetService"/> class.
        /// </summary>
        /// <param name="mapper">Mapper class</param>
        /// <param name="tweetRepository">Tweet repository</param>
        /// <param name="userRepository">User repository</param>
        public TweetService(ITweetRepository tweetRepository, IMapper mapper, IUserRepository userRepository)
        {
            this.tweetRepository = tweetRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get all tweets
        /// </summary>
        /// <returns>Tweets</returns>
        public async Task<IEnumerable<Tweet>> GetAllTweets()
        {
            IEnumerable<Tweet> tweets = await tweetRepository.GetAllTweets();
            if (tweets != null)
            {
                return tweets.OrderByDescending(m => m.DateAndTimeofTweet);
            }

            return tweets;
        }

        /// <summary>
        /// Get user tweets
        /// </summary>
        /// <param name="username">User name</param>
        /// <returns>Tweets</returns>
        public async Task<IEnumerable<Tweet>> GetUserTweets(string username)
        {
            IEnumerable<Tweet> tweets = new List<Tweet>();
            User loggedInUser = await userRepository.GetLoggedInUser();
            if (loggedInUser.EmailId == username)
            {
                tweets = await tweetRepository.GetUserTweets(username);
                return tweets.Count() > 0 ? tweets.OrderByDescending(m => m.DateAndTimeofTweet) : tweets;  
            }

            return tweets;
        }

        /// <summary>
        /// Add new tweet
        /// </summary>
        /// <param name="tweet">Tweet</param>
        /// <returns>Tweet</returns>
        public async Task<Tweet> AddNewTweet(PostNewTweetDto tweet, string username)
        {
            Tweet newTweet = new Tweet();
            User loggedInUser = await userRepository.GetLoggedInUser();
            if (loggedInUser.EmailId == username)
            {
                newTweet = mapper.Map<Tweet>(tweet);
                newTweet.Likes = 0;
                newTweet.Username = username;
                newTweet.Replies = new List<ReplyTweet>();
                newTweet.DateAndTimeofTweet = DateTime.Now;
                newTweet.LikedBy = new string[] { };
                return await tweetRepository.AddNewTweet(newTweet);
            }

            return newTweet;
        }

        /// <summary>
        /// Update user tweet
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="id">Id</param>
        /// <param name="newMessage">New message</param>
        /// <returns>Tweet</returns>
        public async Task<Tweet> UpdateTweet(string username, string id, string newMessage)
        {
            Tweet updatedTweet = new Tweet();
            User loggedUser = await userRepository.GetLoggedInUser();
            if (loggedUser.EmailId == username)
            {
                updatedTweet = await tweetRepository.UpdateTweet(username, id, newMessage);
            }
            return updatedTweet;
        }

        /// <summary>
        /// Delete tweet
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="id">Id</param>
        /// <returns>True, if tweet is deleted successfully, False otherwise</returns>
        public async Task<bool> DeleteTweet(string username, string id)
        {
            User loggedInUser = await userRepository.GetLoggedInUser();
            if (loggedInUser.EmailId == username)
            {
                return await tweetRepository.DeleteTweet(username, id);
            }

            return false;
        }

        /// <summary>
        /// Like tweet
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="id">Id</param>
        /// <returns>Integer value representing number of likes on the tweet</returns>
        public async Task<int> LikeUnlikeTweet(string username, string id)
        {
            User loggedUser = await userRepository.GetLoggedInUser();
            if (loggedUser.EmailId == username)
            {
                return await tweetRepository.LikeTweet(username, id);
            }

            return -1;
        }

        /// <summary>
        /// Reply tweet
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="replyTweet">Reply tweet</param>
        /// <returns>True if reply to tweet saved successfully, False otherwise</returns>
        public async Task<bool> Reply(string username, string id, string message)
        {
            User loggedUser = await userRepository.GetLoggedInUser();
            if (loggedUser.EmailId == username)
            {
                ReplyTweet replyTweet = new ReplyTweet
                {
                    ReplyTweetText = message,
                    Username = username,
                    DateAndTimeOfReply = DateTime.Now
                };

                return await tweetRepository.Reply(id, replyTweet);
            }

            return false;
        }
    }
}
