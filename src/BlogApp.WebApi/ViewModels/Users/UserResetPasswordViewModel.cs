using BlogApp.WebApi.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlogApp.Service.ViewModels.Users
{
    public class UserResetPasswordViewModel
    {
        [Required, Email]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("code")]
        public uint Code { get; set; }

        [Required(ErrorMessage = "Password is required"), MinLength(8), MaxLength(50)]
        //[StrongPassword]
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }
}
