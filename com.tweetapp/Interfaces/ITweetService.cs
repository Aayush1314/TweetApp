using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.tweetapp.DTO;

namespace com.tweetapp.Interfaces
{
    public interface ITweetService
    {
        Task<List<TweetDto>> GetAllTweets();
        Task<List<TweetDto>> GetUserTweets(string username);
        Task<TweetPostDto> PostTweet(TweetPostDto tweetPostDto);
        
        Task<TweetDto> UpdateTweet(TweetDto tweetDto, string username, string id);
        Task<string> DeleteTweet(string username, string id);
        Task<int> LikeTweet(string username, string id);
    }
}

