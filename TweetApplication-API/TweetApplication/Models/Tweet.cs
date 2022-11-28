using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.Models
{
    public class Tweet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Tweet Message is required")]
        [StringLength(144, ErrorMessage = "Tweet must be less than 144 characters!")]
        [BsonElement("tweetMessage")]
        public string TweetMessage { get; set; }

        [BsonElement("dateAndTimeOfTweet")]
        public DateTime DateAndTimeofTweet { get; set; }

        [BsonElement("likes")]
        public int Likes { get; set; }

        [BsonElement("replies")]
        public List<ReplyTweet> Replies { get; set; }

        [BsonElement("likedBy")]
        public string[] LikedBy { get; set; }
    }
}
