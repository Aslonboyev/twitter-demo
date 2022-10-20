namespace BlogApp.WebApi.Models
{
    public class SaveMessage
    {
        public long Id { get; set; }

        public long BlogPostId { get; set; }
        public virtual BlogPost BlogPost { get; set; } = null!;

        public long UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public ICollection<BlogPost> BlogPosts { get; } = null!;
    }
}