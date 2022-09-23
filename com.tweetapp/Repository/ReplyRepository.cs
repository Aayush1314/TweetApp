using System;
using System.Threading.Tasks;
using com.tweetapp.DTO;
using com.tweetapp.Interfaces;
using com.tweetapp.Model;
using com.tweetapp.Services;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
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

        public async Task<TweetDto> PostReply(string username, string tweetId, ReplyDto replyDto)
        {
            var reply = new Reply
            {
                reply = replyDto.reply,
                replyDateTime = DateTime.Now.ToString(),
                username = username,
                refId = ObjectId.Parse(tweetId)
            };
            await _replyCollection.InsertOneAsync( reply);

            await _tweetsCollection.UpdateOneAsync(t => t.Id == ObjectId.Parse(tweetId), Builders<Tweet>.Update.Push("replyList", reply.Id));

            var tweet = await _tweetsCollection.Aggregate().
                Match(t => t.Id == ObjectId.Parse(tweetId)).
                Lookup("Users", "userId", "_id", "user").
                Lookup("Replies", "replyList", "_id", "replies").
                Project(new BsonDocument
                {
                    { "TweetId",1 },
                    { "tweet", 1},
                    {"tweetDate", 1 },
                    {"likeCount",1 },
                    {"tags", 1 },
                    {"user", 1 },
                    {"replies", 1 }
                }).FirstOrDefaultAsync();
            var tweetDto = BsonSerializer.Deserialize<TweetDto>(tweet);
            Console.WriteLine(tweetDto);
            return tweetDto;
        }
    }
}

