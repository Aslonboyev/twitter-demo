using BlogApp.WebApi.Models;
using System.Text.Json.Serialization;

namespace BlogApp.WebApi.ViewModels.SaveMessages
{
    public class SaveMessageCreateViewModel
    {
        [JsonPropertyName("post_id")]
        public long PostId { get; set; }

        public static implicit operator SaveMessage(SaveMessageCreateViewModel model)
        {
            return new SaveMessage
            {
                BlogPostId = model.PostId,
            };
        }
    }
}