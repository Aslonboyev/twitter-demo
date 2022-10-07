using BlogApp.WebApi.Attributes;
using BlogApp.WebApi.Models;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlogApp.WebApi.ViewModels.BlogPosts
{
    public class BlogPostCreateViewModel
    {
        [Required]
        [MaxLength(150)]
        [JsonPropertyName("title")]
        public string Title { get; set; } = String.Empty;

        [Required]
        [MinLength(50)]
        [JsonPropertyName("description")]
        public string Description { get; set; } = String.Empty;

        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; } = String.Empty;

        [JsonPropertyName("subtitle")]
        public string Subtitle { get; set; } = String.Empty;

        [DataType(DataType.Upload)]
        [MaxFileSize(3)]
        [AllowedFileExtensions(new string[] { ".jpg", ".png" })]
        [JsonPropertyName("image")]
        public IFormFile Image { get; set; } = null!;
        
        [Required]
        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        public static implicit operator BlogPost(BlogPostCreateViewModel model)
        {
            return new BlogPost
            {
                Title = model.Title,
                SubTitle = model.Subtitle,
                Type = model.Type,
                Description = model.Description,
                UserId = model.UserId,
            };
        }
    }
}
