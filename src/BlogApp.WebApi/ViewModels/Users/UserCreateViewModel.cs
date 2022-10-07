using BlogApp.WebApi.Attributes;
using BlogApp.WebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.WebApi.ViewModels.Users
{
    public class UserCreateViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50), MinLength(2)]
        [Name]
        public string FirstName { get; set; } = String.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50), MinLength(2)]
        [Name]
        public string LastName { get; set; } = String.Empty;

        //[Required]
        //public string Username { get; set; } = String.Empty;

        [Required, Email]
        public string Email { get; set; } = String.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StrongPassword]
        public string Password { get; set; } = String.Empty;

        [Required(ErrorMessage = "Image is required")]
        [DataType(DataType.Upload)]
        [MaxFileSize(3)]
        [AllowedFileExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile Image { get; set; } = null!;

        public static implicit operator User(UserCreateViewModel model)
        {
            return new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                //Username = model.Username,
                PasswordHash = model.Password,
            };
        }
    }
}