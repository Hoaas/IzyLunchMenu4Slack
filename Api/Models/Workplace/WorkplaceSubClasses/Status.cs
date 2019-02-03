using System;
using Newtonsoft.Json;

namespace Api.Models.Workplace.WorkplaceSubClasses
{
    public class Status
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("code_text")]
        public string CodeText { get; set; }

        [JsonProperty("response_timestamp")]
        public DateTimeOffset ResponseTimestamp { get; set; }
    }
}