using System;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace com.tweetapp.Model
{
    public class User
    {
        public ObjectId Id { get; set; }
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [Required]
        [Phone]
        public string contact { get; set; }
    }
}

