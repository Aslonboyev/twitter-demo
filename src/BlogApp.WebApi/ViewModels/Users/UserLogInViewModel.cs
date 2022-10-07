using BlogApp.WebApi.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlogApp.WebApi.ViewModels.Users
{
    public class UserLogInViewModel
    {
        [Required, Email]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required"), MaxLength(50), MinLength(8)]
        //[StrongPassword]    
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }
}
