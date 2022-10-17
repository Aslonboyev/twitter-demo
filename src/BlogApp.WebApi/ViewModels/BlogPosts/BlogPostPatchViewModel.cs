using BlogApp.WebApi.Attributes;
using BlogApp.WebApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlogApp.WebApi.ViewModels.BlogPosts
{
    public class BlogPostPatchViewModel
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [DataType(DataType.Upload)]
        [MaxFileSize(3)]
        [AllowedFileExtensions(new string[] { ".jpg", ".png" })]
        [JsonPropertyName("image")]
        public IFormFile? Image { get; set; }

        public static implicit operator BlogPostPatchViewModel(BlogPost blogPost)
        {
            return new BlogPostPatchViewModel
            {
                Title = blogPost.Title,
                Description = blogPost.Description,
                Type = blogPost.Type,
            };
        }
    }
}
