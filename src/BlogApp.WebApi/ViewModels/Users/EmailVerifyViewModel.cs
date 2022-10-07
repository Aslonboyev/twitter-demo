using BlogApp.WebApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.WebApi.ViewModels.Users
{
    public class EmailVerifyViewModel
    {
        [Required, Email]
        public string Email { get; set; } = string.Empty;

        [Required]
        public int Code { get; set; }
    }
}
