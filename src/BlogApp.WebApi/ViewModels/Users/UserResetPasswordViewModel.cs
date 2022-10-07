using BlogApp.WebApi.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Service.ViewModels.Users
{
    public class UserResetPasswordViewModel
    {
        [Required, Email]
        public string Email { get; set; } = string.Empty;

        [Required]
        public uint Code { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StrongPassword]
        public string Password { get; set; } = string.Empty;
    }
}
