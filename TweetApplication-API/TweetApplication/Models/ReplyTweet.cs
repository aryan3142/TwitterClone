using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.Models
{
    public class ReplyTweet
    {
        [Required]
        [BsonElement("replyTweetText")]
        public string ReplyTweetText { get; set; }

        //[BsonElement("originalTweetId")]
        //public string OriginalTweetId { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("dateAndTimeOfReply")]
        public DateTime? DateAndTimeOfReply { get; set; }
    }
}
