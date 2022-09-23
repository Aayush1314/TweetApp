using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.tweetapp.DTO;
using com.tweetapp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace com.tweetapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplyController : Controller
    {
        private readonly ILogger<ReplyController> _logger;
        private readonly IReplyService _replyService;

        public ReplyController(ILogger<ReplyController> logger,IReplyService replyService)
        {
            _logger = logger;
            _replyService = replyService;
        }
        
        // POST api/values
        [HttpPost("/api/v1.0/tweets/{username}/reply/{id}")]
        [Authorize]

        public async Task<ActionResult> Post(string username, string id, ReplyDto replyDto)
        {
            try
            {
                var response = await _replyService.PostReply(username, id, replyDto);
                if (response != null)
                {
                    return Ok(response);
                }
                return BadRequest("User/Tweet not Found");
            }
            catch (Exception e)
            {
                _logger.LogTrace(e.StackTrace);
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
        }

    }
}

