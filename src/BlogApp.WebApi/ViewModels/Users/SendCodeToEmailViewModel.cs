using BlogApp.WebApi.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlogApp.WebApi.ViewModels.Users
{
    public class SendCodeToEmailViewModel
    {
        [Required, Email]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
    }
}
