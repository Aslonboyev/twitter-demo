namespace BlogApp.WebApi.Models
{
    public class BlogPost
    {
        public long Id { get; set; }

        public string Title { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;

        public uint ViewCount { get; set; }

        public long PostTypeId { get; set; }
        public virtual PostType PostType { get; set; } = null!;

        public string ImagePath { get; set; } = String.Empty;

        public DateTime CreatedAt { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
