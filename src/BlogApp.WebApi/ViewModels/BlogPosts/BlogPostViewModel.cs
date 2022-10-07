namespace BlogApp.WebApi.ViewModels.BlogPosts
{
    public class BlogPostViewModel
    {
        public ulong Id { get; set; }

        public string Title { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;

        public int ViewCount { get; set; }

        public string Type { get; set; } = String.Empty;

        public string Subtitle { get; set; } = String.Empty;

        public string Image { get; set; } = String.Empty;

        public DateTime CreatedAt { get; set; }

        public ulong UserId { get; set; }
    }
}
