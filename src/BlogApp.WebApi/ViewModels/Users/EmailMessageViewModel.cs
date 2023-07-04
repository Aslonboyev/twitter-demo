using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace BlogApp.WebApi.ViewModels.Users
{
    public class EmailMessageViewModel
    {
        [JsonPropertyName("to")]
        public string To { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; } = string.Empty;

        [JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;
    }
}