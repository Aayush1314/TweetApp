using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.tweetapp.DTO;
using com.tweetapp.Interfaces;
using com.tweetapp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace com.tweetapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger,IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        // GET: AllUsers
        [HttpGet("/api/v1.0/tweets/users/all")]
        public ActionResult<List<UserSearchDto>> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Getting All users");
                var userList = _accountService.GetAllUsers();
                if (userList.Count < 1)
                {
                    _logger.LogWarning("No users in db");
                    return BadRequest("No user found");
                }
                _logger.LogInformation("Users found");
                return Ok(userList);
            }
            catch(Exception e)
            {
                _logger.LogTrace(e.StackTrace);
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
        }

        // Post: Register User
        [HttpPost("/api/v1.0/tweets/register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Modelstate not valid");
                return BadRequest("Details not correct. Please try again.");
            }
            try
            {
                return Ok(await _accountService.Register(registerDto));
            }
            catch (Exception e)
            {
                _logger.LogTrace(e.StackTrace);
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
        }

        [HttpGet("/api/v/1.0/tweets/user/search/{username}")]
        public ActionResult<List<UserSearchDto>> GetUserByUsername(string username)
        {
            try
            {
                var userList = _accountService.GetUserByUsername(username);
                if (userList.Count() == 0)
                {
                    return BadRequest(new { Response = "User Not Found"});
                }
                return Ok(userList);
            }
            catch(Exception e)
            {
                _logger.LogTrace(e.StackTrace);

                return StatusCode(500, new { Message = "Internal Server Error." });
            }
        }

        [HttpPost("/api/v1.0/tweets/login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Details not correct. Please try again.");
                }
                var user = await _accountService.Login(loginDto);
                if (user != null)
                {
                    return Ok(user);
                }
                return Unauthorized("Invalid Caredntials");
            }
            catch (Exception e)
            {
                _logger.LogTrace(e.StackTrace);

                return StatusCode(500, new { Message = "Internal Server Error." });
            }

        }

        [HttpPut("/api/v1.0/tweets/{username}/forgot")]        
        public async Task<ActionResult> ForgotPassword(string username, ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                var response = (await _accountService.ForgotPassword(username, forgotPasswordDto));
                if (response == null)
                {
                    return BadRequest("User not found");
                }
                return Ok(new { Response = response });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
        }
    }
}

