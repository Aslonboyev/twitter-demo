using BlogApp.WebApi.Models;
using BlogApp.WebApi.ViewModels.PostTypes;
using BlogApp.WebApi.ViewModels.Users;
using System.Text.Json.Serialization;

namespace BlogApp.WebApi.ViewModels.BlogPosts
{
    public class BlogPostViewModel
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = String.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = String.Empty;

        [JsonPropertyName("view_count")]
        public uint ViewCount { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; } = String.Empty;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("type")]
        public PostTypeViewModel PostType { get; set; } = null!;

        [JsonPropertyName("user")]
        public UserViewModel User { get; set; } = null!;

        public static implicit operator BlogPostViewModel(BlogPost blogPost)
        {
            return new BlogPostViewModel
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Description = blogPost.Description,
                Image = blogPost.ImagePath,
                ViewCount = blogPost.ViewCount,
                CreatedAt = blogPost.CreatedAt,
                User = (UserViewModel)blogPost.User,
                PostType = (PostTypeViewModel)blogPost.PostType,
            };
        }
    }
}