using System;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace com.tweetapp.DTO
{
    public class ReplyDto
    {
        
        public ObjectId replyId { get; set; }
        [Required]
        [MaxLength(144)]
        public string reply { get; set; }
        [Required]
        public string replyDate { get; set; }

        public int likeCount { get; set; } = 0;
        public string[] tags { get; set; }
        [Required]
        public string username { get; set; }
    
    }
}

