using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.tweetapp.DTO;
using com.tweetapp.Interfaces;
using com.tweetapp.Model;
using Microsoft.AspNetCore.Mvc;

namespace com.tweetapp.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public List<UserSearchDto> GetAllUsers()
        {
            return _accountRepository.GetAllUsers();
        }

        public async Task<UserLoginDto> Login(LoginDto loginDto)
        {
            return await _accountRepository.Login(loginDto);
        }

        public  List<UserSearchDto> GetUserByUsername(string username)
        {
            return _accountRepository.GetUserByUsername(username);
        }

        public async Task<UserLoginDto>  Register(RegisterDto registerDto)
        {
            return await _accountRepository.Register(registerDto);
        }

        public async Task<string> ForgotPassword(string username, ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _accountRepository.GetUserByExactUsername(username);
            if (user == null)
            {
                return null;
            }
            return await _accountRepository.ForgotPassword(username, forgotPasswordDto);
        }
    }
}

