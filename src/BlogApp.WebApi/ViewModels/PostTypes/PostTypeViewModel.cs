using BlogApp.WebApi.Models;

namespace BlogApp.WebApi.ViewModels.PostTypes
{
    public class PostTypeViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; } = String.Empty;

        public static implicit operator PostTypeViewModel(PostType post)
        {
            return new PostTypeViewModel
            {
                Id = post.Id,
                Name = post.Name,
            };
        }
    }
}
