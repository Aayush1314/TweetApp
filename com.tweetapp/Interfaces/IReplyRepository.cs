using System;
using com.tweetapp.DTO;
using System.Threading.Tasks;

namespace com.tweetapp.Interfaces
{
    public interface IReplyRepository
    {
        public Task<TweetDto> PostReply(string userId, string tweetId, ReplyDto replyDto);

    }
}

