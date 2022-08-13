using System;
using com.tweetapp.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace com.tweetapp.Interfaces
{
    public interface ITweetRepository
    {
        Task<List<TweetDto>> GetAllTweets();
        Task<List<TweetDto>> GetUserTweets(string userId);
        Task<TweetPostDto> PostTweet(TweetPostDto tweetPostDto, ObjectId userId);
        Task<TweetDto> GetTweetById(string username, string id);
        Task<TweetDto> UpdateTweet(string username, string id, TweetDto tweetDto);
        Task<string> DeleteTweet( string id);
        Task<int> LikeTweet(string username, string id);
    }
}

