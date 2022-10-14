using BlogApp.WebApi.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlogApp.Service.ViewModels.Users
{
    public class UserPatchViewModel
    {
        [MaxLength(50), MinLength(2)]
        [Name]
        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        [MaxLength(50), MinLength(2)]
        [Name]
        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }

        [MinLength(3)]
        [JsonPropertyName("user_name")]
        public string? UserName { get; set; }

        [Email]
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [MaxLength(50), MinLength(8)]
        //StrongPassword]
        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [DataType(DataType.Upload)]
        [MaxFileSize(1)]
        [AllowedFileExtensions(new string[] { ".jpg", ".png" })]
        [JsonPropertyName("image")]
        public IFormFile? Image { get; set; }
    }
}