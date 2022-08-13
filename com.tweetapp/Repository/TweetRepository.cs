using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.tweetapp.DTO;
using com.tweetapp.Interfaces;
using com.tweetapp.Model;
using com.tweetapp.Services;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace com.tweetapp.Repository
{
    public class TweetRepository : ITweetRepository
    {

        
        private readonly IConfiguration _configuration;
        //private readonly FilterDefinitions _filterDefinitions;
        private readonly IMongoCollection<Tweet> _tweetsCollection;
        private readonly IMongoCollection<User> _usersCollection;
        //private readonly IMongoCollection<Reply> _repliesCollection;

        public TweetRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            MongoClient _client = new MongoClient(_configuration.GetConnectionString("TweetAppConnectionString"));
            _tweetsCollection = _client.GetDatabase("TweetApp").GetCollection<Tweet>("Tweets");
            _usersCollection = _client.GetDatabase("TweetApp").GetCollection<User>("Users");
        }
        public async Task<string> DeleteTweet( string id)
        {
            try
            {
                var tweet = await _tweetsCollection.FindOneAndDeleteAsync(t => t.Id == ObjectId.Parse(id));
                return "Tweet deleted sucessfully";

            }
            catch (Exception e)
            {
                return "Tweet not found";
            }
        }

        public async Task<List<TweetDto>> GetAllTweets()
        {
            var tweets = _tweetsCollection.Aggregate().
                Lookup<Tweet, User, TweetDetailDto>(_usersCollection, a => a.userId, a => a.Id, a => a.user)
                .ToEnumerable().
                SelectMany(t => t.user.Select(u => new TweetDto
                {
                    TweetId = t.Id,
                    tweet = t.tweet,
                    tweetDate = t.tweetDate,
                    likeCount = t.likeCount,
                    tags = t.tags,                                    
                    username = u.username
                }))
                .ToList();
            //var tweets = _tweetsCollection.AsQueryable().ToList();

            //foreach (var tweet in tweets)
            //{
            //    Console.WriteLine(tweet);
            //}
            return tweets;
        }

        public async Task<TweetDto> GetTweetById(string username, string tweetId)
        {
            var tweet = await _tweetsCollection.Find(t => t.Id == ObjectId.Parse(tweetId)).FirstOrDefaultAsync();
            if (tweet == null)
            {
                return null;
            } 
            var tweetDto = new TweetDto
            {
                TweetId = tweet.Id,
                tweet = tweet.tweet,
                tags = tweet.tags,
                tweetDate = tweet.tweetDate,
                username = username,
                likeCount = tweet.likeCount,

            };
            return tweetDto;
        }

        public async Task<List<TweetDto>> GetUserTweets(string userId)
        {
             var tweets =  _tweetsCollection.Aggregate().
                Match(t=>t.userId == ObjectId.Parse(userId))
                .Lookup<Tweet, User, TweetDetailDto>(_usersCollection, a => a.userId, a => a.Id, a => a.user)
                .ToEnumerable()
                .SelectMany(t => t.user.Select(u => new TweetDto
                {
                    tweet = t.tweet,
                    tweetDate = t.tweetDate,
                    likeCount = t.likeCount,
                    tags = t.tags,
                    username = u.username
                }))
                .ToList();

            return tweets;
                 
        }

        public async Task<int> LikeTweet(string username, string id)
        {
            var tweet = await _tweetsCollection.Find(t => t.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
            if (tweet == null)
            {
                return -1;
            }
            await _tweetsCollection.FindOneAndUpdateAsync(t => t.Id == ObjectId.Parse(id), Builders<Tweet>.Update.Set(tweet => tweet.likeCount, tweet.likeCount + 1));
            return tweet.likeCount + 1;    
        }

        public async Task<TweetPostDto> PostTweet(TweetPostDto tweetPostDto, ObjectId userId)
        {
            Tweet newTweet = new Tweet()
            {
                tweet = tweetPostDto.tweet,
                likeCount = tweetPostDto.likeCount,
                tweetDate = DateTime.Now.ToString(),
                tags = tweetPostDto.tags,
                userId = userId
            };
            await _tweetsCollection.InsertOneAsync(newTweet);
            return tweetPostDto;
            
        }

        public async Task<TweetDto> UpdateTweet(string username, string id, TweetDto tweetDto)
        {
            await _tweetsCollection.FindOneAndUpdateAsync(t => t.Id == ObjectId.Parse(id), Builders<Tweet>.Update.Set(tweet => tweet.tweet, tweetDto.tweet));
            return tweetDto;
        }
    }
}

