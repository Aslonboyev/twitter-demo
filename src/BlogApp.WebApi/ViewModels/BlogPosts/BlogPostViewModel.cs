using BlogApp.WebApi.Models;
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

        [JsonPropertyName("type-id")]
        public long TypeId { get; set; }

        //[JsonPropertyName("image")]
        //public string Image { get; set; } = String.Empty;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        public static implicit operator BlogPostViewModel(BlogPost blogPost)
        {
            return new BlogPostViewModel
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                TypeId = blogPost.PostTypeId,
                Description = blogPost.Description,
                UserId = blogPost.UserId,
                //Image = blogPost.ImagePath,
                ViewCount = blogPost.ViewCount,
                CreatedAt = blogPost.CreatedAt,
            };
        }
    }
}