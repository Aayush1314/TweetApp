using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using com.tweetapp.DTO;
using com.tweetapp.Interfaces;
using com.tweetapp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace com.tweetapp.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        //private readonly FilterDefinitions _filterDefinitions;
        private readonly IMongoCollection<Tweet> _tweetsCollection;
        private readonly IMongoCollection<User> _usersCollection;
        //private readonly IMongoCollection<Reply> _repliesCollection;

        public AccountRepository(ITokenService tokenService, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _configuration = configuration;
            MongoClient _client = new MongoClient(_configuration.GetConnectionString("TweetAppConnectionString"));
            _tweetsCollection = _client.GetDatabase("TweetApp").GetCollection<Tweet>("Tweets");
            _usersCollection = _client.GetDatabase("TweetApp").GetCollection<User>("Users");
        }

        public List<UserSearchDto> GetAllUsers()
        {
            //var users = _usersCollection.AsQueryable().ToList();
            //foreach(var u in users)
            //{
            //    Console.WriteLine(u);
            //}
            return _usersCollection.AsQueryable().Select(u=> new UserSearchDto {
                   userID = u.Id,
                   username = u.username,
                   firstName = u.firstName,
                   lastName = u.lastName,
                   email = u.email,
                   contact = u.contact
            }).ToList();
        }

        public async Task<string> GetUserId(string username)
        {
            var user = await _usersCollection.Find(u => u.username == username).FirstOrDefaultAsync();
            if (user != null)
            {
                return (user.Id).ToString();
            }
            return null;
        }

        public async Task<UserLoginDto> Login(LoginDto loginDto)
        {
            var user = await _usersCollection.Find(u => loginDto.username==u.username).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }

            try
            {
                using var hmac = new HMACSHA512(user.PasswordSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != user.PasswordHash[i]) return null;
                }

                return new UserLoginDto
                {
                    userID = user.Id,
                    username = loginDto.username,
                    firstName = user.firstName,
                    lastName = user.lastName,
                    email = user.email,
                    contact = user.contact,
                    token = _tokenService.CreateToken(user)
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);

            }

        }

        public async Task<UserSearchDto> GetUserByExactUsername(string username)
        {
            var user = await _usersCollection.Find(u => u.username == username).FirstOrDefaultAsync();

            var userSearchDto = new UserSearchDto
            {
                userID = user.Id,
                username = user.username,
                firstName = user.firstName,
                lastName = user.lastName,
                contact = user.contact,
                email = user.email
            };
            return userSearchDto;
        }

        public List<UserSearchDto> GetUserByUsername(string username)
        {
            var queryExpression = new BsonRegularExpression(new Regex(username, RegexOptions.None));
            FilterDefinition<User> filterDefinition = Builders<User>.Filter.Regex("username", queryExpression);
            return _usersCollection.Find(filterDefinition).ToEnumerable().Select(u => new UserSearchDto
            {
                userID = u.Id,
                username = u.username,
                firstName = u.firstName,
                lastName = u.lastName,
                contact = u.contact,
                email = u.email
            }).ToList();
        }

        public async Task<UserLoginDto> Register(RegisterDto registerDto)
        {
            using var hmac = new HMACSHA512();
            var user = new User
            {
                username = registerDto.username,
                firstName = registerDto.firstName,
                lastName = registerDto.lastName,
                contact = registerDto.contact,
                email = registerDto.email,                
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password)),
                PasswordSalt = hmac.Key
            };
            await _usersCollection.InsertOneAsync(user);

            return new UserLoginDto
            {
                userID = user.Id,
                firstName = user.firstName,
                lastName = user.lastName,
                email = user.email,
                contact = user.contact,
                token = _tokenService.CreateToken(user),
                username = registerDto.username,
            };
        }

        public async Task<string> ForgotPassword(string username, ForgotPasswordDto forgotPasswordDto)
        {
            
            if (forgotPasswordDto.confirmPassword == forgotPasswordDto.newPassword)
            {
                using var hmac = new HMACSHA512();

                await _usersCollection.UpdateOneAsync(u => u.username==username,
                                                Builders<User>
                                                .Update
                                                .Set(u=>u.PasswordHash , hmac.ComputeHash(Encoding.UTF8.GetBytes(forgotPasswordDto.newPassword)))
                                                .Set(u=>u.PasswordSalt, hmac.Key)
                                                );
                return "Password updated";
            }
            return "Password does not match. Try again!!!";

        }
    }
}

