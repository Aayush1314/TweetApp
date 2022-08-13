using System;
using System.Threading.Tasks;
using com.tweetapp.DTO;
using com.tweetapp.Interfaces;
using com.tweetapp.Model;
using com.tweetapp.Services;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace com.tweetapp.Repository
{
    public class ReplyRepository:IReplyRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoCollection<Tweet> _tweetsCollection;
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IMongoCollection<Reply> _replyCollection;

        public ReplyRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            MongoClient _client = new MongoClient(_configuration.GetConnectionString("TweetAppConnectionString"));
            _tweetsCollection = _client.GetDatabase("TweetApp").GetCollection<Tweet>("Tweets");
            _usersCollection = _client.GetDatabase("TweetApp").GetCollection<User>("Users");
            _replyCollection = _client.GetDatabase("TweetApp").GetCollection<Reply>("Replies");

        }

        public async Task<string> PostReply(string userId, string tweetId, ReplyDto replyDto)
        {
            await _replyCollection.InsertOneAsync( new Reply
            {
                likeCount = 0,
                reply = replyDto.reply,
                replyDateTime = DateTime.Now.ToString(),
                tags = replyDto.tags,
                userId = ObjectId.Parse(userId),
                refId = ObjectId.Parse(tweetId)
            });
            return "Reply added successfully";
        }
    }
}

