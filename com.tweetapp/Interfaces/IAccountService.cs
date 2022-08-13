using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.tweetapp.DTO;
using com.tweetapp.Model;
using Microsoft.AspNetCore.Mvc;

namespace com.tweetapp.Interfaces
{
    public interface IAccountService
    {
        Task<UserLoginDto> Register(RegisterDto registerDto);
        Task<UserLoginDto> Login(LoginDto loginDto);
        List<UserSearchDto> GetUserByUsername(string username);
        List<UserSearchDto> GetAllUsers();
        Task<string> ForgotPassword(string username, ForgotPasswordDto forgotPasswordDto);
    }
}

