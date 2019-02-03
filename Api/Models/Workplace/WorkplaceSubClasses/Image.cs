using System;
using Newtonsoft.Json;

namespace Api.Models.Workplace.WorkplaceSubClasses
{
    public class Image
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public dynamic Type { get; set; }

        [JsonProperty("title_key")]
        public dynamic TitleKey { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("image_url")]
        public Uri ImageUrl { get; set; }
    }
}