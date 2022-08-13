using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace com.tweetapp.DTO
{
    public class UserLoginDto
    {
        [Required]
        public ObjectId userID { get; set; }
        [Required]
        public string username { get; set; }
        public string token { get; set; }        
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }        
        [Required]
        [Phone]
        public string contact { get; set; }

    }
}

