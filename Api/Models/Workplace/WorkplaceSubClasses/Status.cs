using System;
using System.Text.Json.Serialization;

namespace Api.Models.Workplace.WorkplaceSubClasses
{
    public class Status
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("code_text")]
        public string CodeText { get; set; }

        [JsonPropertyName("response_timestamp")]
        public string ResponseTimestamp { get; set; }
    }
}