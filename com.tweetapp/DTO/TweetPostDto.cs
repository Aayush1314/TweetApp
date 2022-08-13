using System;
using System.ComponentModel.DataAnnotations;

namespace com.tweetapp.DTO
{
    public class TweetPostDto
    {
        [Required]
        [MaxLength(144)]
        public string tweet { get; set; }
        [Required]
        public string tweetDate { get; set; }

        public int likeCount { get; set; } = 0;
        public string[] tags { get; set; }
        [Required]
        public string username { get; set; }
    }
}

