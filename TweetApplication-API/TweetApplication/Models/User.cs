using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Please Enter First Name!")]
        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name!")]
        [BsonElement("lastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Email ID!")]
        [EmailAddress]
        [Key]
        [BsonElement("emailId")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Please Enter Password!")]
        [StringLength(16, ErrorMessage = "Password length must be between 6 to 16", MinimumLength = 6)]
        [BsonElement("password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Enter Date Of Birth!")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [BsonElement("dateofbirth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please Enter Gender!")]
        [BsonElement("gender")]
        public string Gender { get; set; }
    }
}
