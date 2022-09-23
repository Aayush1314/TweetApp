using System;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace com.tweetapp.Model
{
    public class Reply
    {
        public ObjectId Id { get; set; }
        [Required]
        [MaxLength(144)]
        public string reply { get; set; }
        public string replyDateTime { get; set; }
        public ObjectId refId { get; set; }
        public string username { get; set; }
    }
}

