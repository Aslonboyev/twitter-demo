using BlogApp.WebApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.WebApi.ViewModels.Users
{
    public class SendToEmail
    {
        [Required, Email]
        public string Email { get; set; } = string.Empty;
    }
}
