namespace BlogApp.WebApi.Models
{
    public class PostType
    {
        public long Id { get; set; }

        public string Name { get; set; } = String.Empty;

        public DateTime CreatedAt { get; set; }

        public ICollection<BlogPost> BlogPosts { get; } = new List<BlogPost>();
    }
}