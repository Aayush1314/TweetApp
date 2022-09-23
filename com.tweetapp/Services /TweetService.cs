using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.tweetapp.DTO;
using com.tweetapp.Interfaces;
using com.tweetapp.Model;
using MongoDB.Bson;

namespace com.tweetapp.Services
{
    public class TweetService : ITweetService
    {
        private readonly ITweetRepository _tweetRepository;
        private readonly IAccountRepository _accountRepository;

        public TweetService(ITweetRepository tweetRepository, IAccountRepository accountRepository)
        {
            _tweetRepository = tweetRepository;
            _accountRepository = accountRepository;
        }
        public async Task<string> DeleteTweet(string username, string id)
        {
            var getUser = _accountRepository.GetUserByExactUsername(username);
            if (getUser.Result == null)
            {
                return null;
            }
            return await _tweetRepository.DeleteTweet(id);
        }

        public Task<List<TweetDto>> GetAllTweets()
        {
            return _tweetRepository.GetAllTweets();
        }

        public async Task<List<TweetDto>> GetUserTweets(string username)
        {
            string userId  = await _accountRepository.GetUserId(username);
            var tweets = await _tweetRepository.GetUserTweets(userId);
            return tweets;
        }

        public async Task<int> LikeTweet(string username, string id)
        {
            var user = await _accountRepository.GetUserByExactUsername(username);
            if (user == null)
            {
                return -1;
            }
            return await _tweetRepository.LikeTweet(username, id);

        }

        public async Task<TweetPostDto> PostTweet(TweetPostDto tweetPostDto)
        {
            string userId = await _accountRepository.GetUserId(tweetPostDto.username);
            if (userId != null)
            {
                return await _tweetRepository.PostTweet(tweetPostDto, ObjectId.Parse(userId)); 
            }
            return null;
        }

        public async Task<string> UpdateTweet(EditTweetDto editTweetDto, string username, string id)
        {
            return await _tweetRepository.UpdateTweet(username, id, editTweetDto);
            
        }
    }
}

