using System;
using System.Threading.Tasks;
using com.tweetapp.DTO;
using com.tweetapp.Interfaces;
using MongoDB.Bson;

namespace com.tweetapp.Services
{
    public class ReplyService:IReplyService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IReplyRepository _replyRepository;
        private readonly ITweetRepository _tweetRepository;

        public ReplyService(IAccountRepository accountRepository, IReplyRepository replyRepository, ITweetRepository tweetRepository)
        {
            _accountRepository = accountRepository;
            _replyRepository = replyRepository;
            _tweetRepository = tweetRepository;
        }

        public async Task<TweetDto> PostReply(string username, string tweetId, ReplyDto replyDto)
        {
            var user = await _accountRepository.GetUserByExactUsername(username);
            var tweet = await _tweetRepository.GetTweetById(username, tweetId);
            if (user == null || tweet == null)
            {
                return null;
            }

            return await _replyRepository.PostReply(user.username, tweetId, replyDto);
        }
    }
}

