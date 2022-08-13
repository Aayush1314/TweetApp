using System;
using com.tweetapp.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using com.tweetapp.Model;
using System.Collections.Generic;
using MongoDB.Bson;

namespace com.tweetapp.Interfaces
{
    public interface IAccountRepository
    {
        Task<UserLoginDto> Register(RegisterDto registerDto);
        Task<UserLoginDto> Login(LoginDto loginDto);
        Task<string> GetUserId(string username);
        List<UserSearchDto> GetAllUsers();
        List<UserSearchDto> GetUserByUsername(string username);
        Task<UserSearchDto> GetUserByExactUsername(string username);
        Task<string> ForgotPassword(string username, ForgotPasswordDto forgotPasswordDto);

    }
}

