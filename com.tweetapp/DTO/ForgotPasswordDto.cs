using System;
using System.ComponentModel.DataAnnotations;

namespace com.tweetapp.DTO
{
    public class ForgotPasswordDto
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string newPassword { get; set; }
        [Required]
        public string confirmPassword { get; set; }
    }
}

