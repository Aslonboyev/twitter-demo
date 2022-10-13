using BlogApp.WebApi.Enums;
using BlogApp.WebApi.ViewModels.BlogPosts;

namespace BlogApp.WebApi.Models
{
    public class BlogPost
    {
        public long Id { get; set; }

        public string Title { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;

        public uint ViewCount { get; set; }

        public string Type { get; set; } = String.Empty;

        //public string ImagePath { get; set; } = String.Empty;

        public ItemState ItemState { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; set; } = null!;

    }
}
