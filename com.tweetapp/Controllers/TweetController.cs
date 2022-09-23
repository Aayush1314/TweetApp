using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.tweetapp.DTO;
using com.tweetapp.Interfaces;
using com.tweetapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace com.tweetapp.Controllers
{
    [ApiController]
    public class TweetController : Controller
    {
        private readonly ITweetService _tweetService;
        private readonly ILogger<TweetController> _logger;

        public TweetController(ILogger<TweetController> logger, ITweetService tweetService)
        {
            _logger = logger;
            _tweetService = tweetService;
        }

        [HttpGet("/api/v1.0/tweets/all")]
        [Authorize]
        public async Task<ActionResult<List<TweetDto>>> GetAllTweets()
        {
            try
            {
                var tweetList = await _tweetService.GetAllTweets();
                if (tweetList.Count < 0)
                {
                    return BadRequest(new { Response = "No tweet found." });
                }
                return Ok(tweetList);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _logger.LogTrace(e.StackTrace);
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
        }

        // POST PostTweets
        [HttpPost("/api/v1.0/tweets/{username}/add")]
        [Authorize]
        public async Task<ActionResult> Post(TweetPostDto tweetPostDto, string username)
        {
            try
            {
                if ((await _tweetService.PostTweet(tweetPostDto)) != null)
                {
                    return Ok(new { Status = "Tweet Added" });
                }
                return BadRequest(new { status = "User Not Found" });
            }
            catch (Exception e)
            {
                _logger.LogTrace(e.StackTrace);
                return StatusCode(500, new { Message = "Internal Server Error." });
            }

        }

        [HttpPut("/api/v1.0/tweets/{username}/update/{id}")]
        [Authorize]
        public async Task<ActionResult<string>> UpdateTweet(string username, string id, EditTweetDto editTweetDto) {

            try
            {
                //Find User bu Username

                //Update Tweet
                await _tweetService.UpdateTweet(editTweetDto, username, id);
                return Ok(editTweetDto);
            }
            catch(Exception e)
            {
                _logger.LogTrace(e.StackTrace);
                return StatusCode(500, new { Message = "Internal Server Error." });
            }


        }

        [HttpPut("/api/v1.0/tweets/{username}/like/{id}")]
        [Authorize]
        public async Task<ActionResult> LikeTweet(string username, string id)
        {
            try
            {
                int likeCount = await _tweetService.LikeTweet(username, id);
                if (likeCount < 0)
                {
                    return BadRequest(new {Response = "User/Tweet not found"});
                }
                return Ok(new { Response = "You liked this!!!"});
            }
            catch (Exception e)
            {
                _logger.LogTrace(e.StackTrace);
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
        }

        [HttpGet("/api/v1.0/tweets/{username}")]
        [Authorize]
        public async Task<ActionResult<List<TweetDto>>> GetUserTweets(string username)
        {
            try
            {
                return await _tweetService.GetUserTweets(username);
            }
            catch(Exception e)
            {
                _logger.LogTrace(e.StackTrace);
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
        }

        [HttpDelete("/api/v1.0/tweets/{username}/delete/{id}")]
        [Authorize]
        public async Task<ActionResult<string>> DeleteTweet(string username, string id)
        {
            try
            {
                var response = await _tweetService.DeleteTweet(username, id);
                if (response == null)
                {
                    return BadRequest("User not found");
                }
                return Ok(new {message=response});
            }
            catch (Exception e)
            {
                _logger.LogTrace(e.StackTrace);
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
        }

    }
}

