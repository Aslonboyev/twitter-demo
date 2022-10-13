using BlogApp.WebApi.Models;
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

        [JsonPropertyName("image")]
        public IFormFile? Image { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        public static implicit operator BlogPostPatchViewModel(BlogPost blogPost)
        {
            return new BlogPostPatchViewModel
            {
                Title = blogPost.Title,
                Description = blogPost.Description,
                UserId = blogPost.UserId,
            };
        }
    }
}
