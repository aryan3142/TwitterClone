using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.Models.Dtos
{
    public class PostNewTweetDto
    {
        [Required(ErrorMessage = "Tweet Message is required")]
        [StringLength(144, ErrorMessage = "Tweet must be less than 144 characters!")]
        [BsonElement("tweetMessage")]
        public string TweetMessage { get; set; }
    }
}
