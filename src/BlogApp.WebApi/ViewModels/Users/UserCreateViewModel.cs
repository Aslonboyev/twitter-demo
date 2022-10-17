using BlogApp.WebApi.Attributes;
using BlogApp.WebApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlogApp.WebApi.ViewModels.Users
{
    public class UserCreateViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50), MinLength(2)]
        [Name]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = String.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50), MinLength(2)]
        [Name]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = String.Empty;

        [Required, MinLength(3)]
        [JsonPropertyName("user_name")]
        public string UserName { get; set; } = String.Empty;

        [Required, Email]
        [JsonPropertyName("email")]
        public string Email { get; set; } = String.Empty;

        [Required(ErrorMessage = "Password is required"), MaxLength(50), MinLength(8)]
        //[StrongPassword]
        [JsonPropertyName("password")]
        public string Password { get; set; } = String.Empty;

        public static implicit operator User(UserCreateViewModel model)
        {
            return new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.UserName,
                PasswordHash = model.Password,
            };
        }
    }
}