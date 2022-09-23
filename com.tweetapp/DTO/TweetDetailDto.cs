using System;
using System.Collections.Generic;
using com.tweetapp.Model;

namespace com.tweetapp.DTO
{
    public class TweetDetailDto : Tweet
    {
        public List<User> user { get; set; }
    }
}

