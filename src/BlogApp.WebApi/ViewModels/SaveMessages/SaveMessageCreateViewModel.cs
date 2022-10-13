using BlogApp.WebApi.Models;
using System.Text.Json.Serialization;

namespace BlogApp.WebApi.ViewModels.SaveMessages
{
    public class SaveMessageCreateViewModel
    {
        [JsonPropertyName("post_id")]
        public long PostId { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        public static implicit operator SaveMessage(SaveMessageCreateViewModel model)
        {
            return new SaveMessage
            {
                PostId = model.PostId,
                UserId = model.UserId,
            };
        }
    }
}