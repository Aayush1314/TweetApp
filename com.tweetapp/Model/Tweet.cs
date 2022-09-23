using System;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace com.tweetapp.Model
{
    public class Tweet
    {
        public ObjectId Id { get; set; }
        [Required]
        [MaxLength(144)]
        public string tweet { get; set; }
        public string tweetDate { get; set; }

        public ObjectId userId { get; set; }
        public List<ObjectId> replyList { get; set; } = new List<ObjectId>();
        public int likeCount { get; set; }
        public string[] tags { get; set; }
    }
}

