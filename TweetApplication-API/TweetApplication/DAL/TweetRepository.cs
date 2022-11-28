using com.tweetapp.Controllers;
using com.tweetapp.Models;
using com.tweetapp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.DAL
{
    public class TweetRepository : ITweetRepository
    {
        /// <summary>
        /// Configuration interface
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="TweetRepository"/> class.
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public TweetRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Get all tweets
        /// </summary>
        /// <returns>Tweets</returns>
        public async Task<IEnumerable<Tweet>> GetAllTweets()
        {
            MongoClient dbClient = new MongoClient(configuration.GetConnectionString("TweetAppCon"));
            var dbList = await dbClient.GetDatabase("TweetAppDb").GetCollection<Tweet>("Tweet").Find(t => true).ToListAsync();
            return dbList;
        }

        /// <summary>
        /// Get user tweets
        /// </summary>
        /// <param name="username">User name</param>
        /// <returns>Tweets</returns>
        public async Task<IEnumerable<Tweet>> GetUserTweets(string username)
        {
            MongoClient dbClient = new MongoClient(configuration.GetConnectionString("TweetAppCon"));
            var filter = Builders<Tweet>.Filter.Eq("username", username);
            var dbList = await dbClient.GetDatabase("TweetAppDb").GetCollection<Tweet>("Tweet").Find(filter).ToListAsync();
            return dbList;
        }

        /// <summary>
        /// Add new tweet
        /// </summary>
        /// <param name="tweet">Tweet</param>
        /// <returns>Tweet</returns>
        public async Task<Tweet> AddNewTweet(Tweet tweet)
        {
            MongoClient dbClient = new MongoClient(configuration.GetConnectionString("TweetAppCon"));
            await dbClient.GetDatabase("TweetAppDb").GetCollection<Tweet>("Tweet").InsertOneAsync(tweet);
            return tweet;
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
            MongoClient dbClient = new MongoClient(configuration.GetConnectionString("TweetAppCon"));
            var filter = Builders<Tweet>.Filter.Eq(t => t.Id, id);
            var update = Builders<Tweet>.Update.Set("tweetMessage", newMessage);
            await dbClient.GetDatabase("TweetAppDb").GetCollection<Tweet>("Tweet").FindOneAndUpdateAsync(filter, update);
            return dbClient.GetDatabase("TweetAppDb").GetCollection<Tweet>("Tweet").Find(filter).FirstOrDefault();
        }

        /// <summary>
        /// Delete tweet
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="id">Id</param>
        /// <returns>True, if tweet is deleted successfully, False otherwise</returns>
        public async Task<bool> DeleteTweet(string username, string id)
        {
            MongoClient dbClient = new MongoClient(configuration.GetConnectionString("TweetAppCon"));
            var filter = Builders<Tweet>.Filter.Eq(t => t.Id, id) & Builders<Tweet>.Filter.Eq(t => t.Username, username);
            var res = await dbClient.GetDatabase("TweetAppDb").GetCollection<Tweet>("Tweet").DeleteOneAsync(filter);
            return res.DeletedCount > 0;
        }

        /// <summary>
        /// Like tweet
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="id">Id</param>
        /// <returns>Integer value representing number of likes on the tweet</returns>
        public async Task<int> LikeTweet(string username, string id)
        {
            MongoClient dbClient = new MongoClient(configuration.GetConnectionString("TweetAppCon"));
            var tweetDetail = await dbClient.GetDatabase("TweetAppDb").GetCollection<Tweet>("Tweet").Find(t => t.Id == id).FirstOrDefaultAsync();
            int likes = tweetDetail.Likes;
            bool isAlreadyLiked = tweetDetail.LikedBy.Contains(username); ;
            if (isAlreadyLiked)
            {
                likes = likes - 1;
                var pushElement = Builders<Tweet>.Update.Combine(
                   Builders<Tweet>.Update.Pull(x => x.LikedBy, username),
                   Builders<Tweet>.Update.Set(x => x.Likes, likes)
                );

                await dbClient.GetDatabase("TweetAppDb").GetCollection<Tweet>("Tweet").UpdateOneAsync(s => s.Id == id, pushElement);
            }
            else
            {
                likes = likes + 1;
                var pushElement = Builders<Tweet>.Update.Combine(
                    Builders<Tweet>.Update.Push(x => x.LikedBy, username),
                    Builders<Tweet>.Update.Set(x => x.Likes, likes)
                    );
                pushElement.Set(t => t.Likes, likes);
                await dbClient.GetDatabase("TweetAppDb").GetCollection<Tweet>("Tweet").UpdateOneAsync(s => s.Id == id, pushElement);
            }
            return likes;
        }

        /// <summary>
        /// Reply tweet
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="replyTweet">Reply tweet</param>
        /// <returns>True if reply to tweet saved successfully, False otherwise</returns>
        public async Task<bool> Reply(string id, ReplyTweet replyTweet)
        {
            MongoClient dbClient = new MongoClient(configuration.GetConnectionString("TweetAppCon"));
            var filter = Builders<Tweet>.Filter.Eq(t => t.Id, id);
            var pushElement = Builders<Tweet>.Update.Push(t => t.Replies, replyTweet);
            var result = await dbClient.GetDatabase("TweetAppDb").GetCollection<Tweet>("Tweet").UpdateOneAsync(filter, pushElement);
            return result.ModifiedCount > 0;
        }
    }
}
