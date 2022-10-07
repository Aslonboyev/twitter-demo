using System.Text.Json.Serialization;

namespace BlogApp.WebApi.ViewModels.Users
{
    public class EmailMessageViewModel
    {
        [JsonPropertyName("to")]
        public string To { get; set; } = string.Empty;

        [JsonPropertyName("body")]
        public int Body { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;
    }
}