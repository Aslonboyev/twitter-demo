using BlogApp.WebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.WebApi.ViewModels.PostTypes
{
    public class PostTypeCreateViewModel
    {
        [Required]
        public string Name { get; set; } = String.Empty;

        public static implicit operator PostType(PostTypeCreateViewModel model)
        {
            return new PostType
            {
                Name = model.Name,
            };
        }
    }
}
