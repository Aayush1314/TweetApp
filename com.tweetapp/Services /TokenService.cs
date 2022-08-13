using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using com.tweetapp.Interfaces;
using com.tweetapp.Model;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace com.tweetapp.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _Key;

        public TokenService(IConfiguration config)
        {
            _Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.username)
            };
            var creds = new SigningCredentials(_Key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDiscriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDiscriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}

