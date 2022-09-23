using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using com.tweetapp.Model;
using MongoDB.Bson;

namespace com.tweetapp.DTO
{
    public class TweetDto
    {
        public ObjectId _id { get; set; }
        [Required]
        [MaxLength(144)]
        public string tweet { get; set; }
        [Required]
        public string tweetDate { get; set; }

        public int likeCount { get; set; } = 0;
        public string[] tags { get; set; }
        public List<User> user { get; set; }
        public List<Reply> replies { get; set; }
        
    }
}

