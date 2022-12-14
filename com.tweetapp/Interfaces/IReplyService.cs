using System;
using System.Threading.Tasks;
using com.tweetapp.DTO;

namespace com.tweetapp.Interfaces
{
    public interface IReplyService
    {
        public Task<TweetDto> PostReply(string username, string tweetId, ReplyDto replyDto);
    }
}

