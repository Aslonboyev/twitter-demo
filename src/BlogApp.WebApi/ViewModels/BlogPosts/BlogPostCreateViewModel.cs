using BlogApp.WebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.WebApi.ViewModels.BlogPosts
{
    public class BlogPostCreateViewModel
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = String.Empty;

        [Required]
        public string Type { get; set; } = String.Empty;

        public string Subtitle { get; set; } = String.Empty;

        public IFormFile Image { get; set; } = null!;

        [Required]
        [MinLength(50)]
        public string Description { get; set; } = String.Empty;

        [Required]
        public long UserId { get; set; }

        public static implicit operator BlogPost(BlogPostCreateViewModel model)
        {
            return new BlogPost
            {
                Title = model.Title,
                Description = model.Description,
                UserId = model.UserId,
            };
        }
    }
}
