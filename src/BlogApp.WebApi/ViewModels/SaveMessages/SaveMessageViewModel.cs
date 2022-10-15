using BlogApp.WebApi.Models;
using System.Text.Json.Serialization;

namespace BlogApp.WebApi.ViewModels.SaveMessages
{
    public class SaveMessageViewModel
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("post_id")]
        public long PostId { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        public static implicit operator SaveMessageViewModel(SaveMessage message)
        {
            return new SaveMessageViewModel
            {
                Id = message.Id,
                PostId = message.BlogPostId,
                UserId = message.UserId,
            };
        }
    }
}
