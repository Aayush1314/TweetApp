using System;
using com.tweetapp.Model;

namespace com.tweetapp.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}

