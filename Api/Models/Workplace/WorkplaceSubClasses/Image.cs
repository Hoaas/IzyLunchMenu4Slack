using System;
using System.Text.Json.Serialization;

namespace Api.Models.Workplace.WorkplaceSubClasses
{
    public class Image
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public dynamic Type { get; set; }

        [JsonPropertyName("title_key")]
        public dynamic TitleKey { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("image_url")]
        public Uri ImageUrl { get; set; }
    }
}