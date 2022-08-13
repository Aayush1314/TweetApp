using System;
using System.ComponentModel.DataAnnotations;

namespace com.tweetapp.DTO
{
    public class RegisterDto
    {


        public string firstName { get; set; }     
        public string lastName { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string contact { get; set; }


    }
}

