using System;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace com.tweetapp.DTO
{
    public class UserSearchDto
    {
        [Required]
        public ObjectId userID { get; set; }

        [Required]
        public string username { get; set; }
        
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

