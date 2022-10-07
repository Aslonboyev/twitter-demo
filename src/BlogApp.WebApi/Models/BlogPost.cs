using BlogApp.WebApi.ViewModels.BlogPosts;

namespace BlogApp.WebApi.Models
{
    public class BlogPost
    {
        public long Id { get; set; }

        public string Title { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;

        public int ViewCount { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public long UserId { get; set; }

        public User User { get; set; } = null!;

        public static implicit operator BlogPostViewModel(BlogPost blogPost)
        {
            return new BlogPostViewModel
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Description = blogPost.Description,
                UserId = blogPost.UserId,
                ViewCount = blogPost.ViewCount,
                CreatedAt = blogPost.CreatedAt,
            };
        }
    }
}
