
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace BlogApp.WebApi.ViewModels.Users
{
    public class EmailMessageViewModel
    {
        [JsonPropertyName("to")]
        public string To { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "body")]
        public int Body { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;
    }
}