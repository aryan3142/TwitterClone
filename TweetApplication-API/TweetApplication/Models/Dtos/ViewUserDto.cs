using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.Models.Dtos
{
    public class ViewUserDto
    {
        [BsonElement("emailId")]
        public string EmailId { get; set; }

        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [BsonElement("dateofbirth")]
        public DateTime? DateOfBirth { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }
    }
}
