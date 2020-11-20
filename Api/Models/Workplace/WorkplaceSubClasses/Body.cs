using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Api.Models.Workplace.WorkplaceSubClasses
{
    public class Body
    {
        [JsonPropertyName("current_page")]
        public long CurrentPage { get; set; }

        [JsonPropertyName("data")]
        public List<Datum> Data { get; set; }

        [JsonPropertyName("first_page_url")]
        public Uri FirstPageUrl { get; set; }

        [JsonPropertyName("from")]
        public long From { get; set; }

        [JsonPropertyName("last_page")]
        public long LastPage { get; set; }

        [JsonPropertyName("last_page_url")]
        public Uri LastPageUrl { get; set; }

        [JsonPropertyName("next_page_url")]
        public dynamic NextPageUrl { get; set; }

        [JsonPropertyName("path")]
        public Uri Path { get; set; }

        [JsonPropertyName("per_page")]
        public long PerPage { get; set; }

        [JsonPropertyName("prev_page_url")]
        public dynamic PrevPageUrl { get; set; }

        [JsonPropertyName("to")]
        public long To { get; set; }

        [JsonPropertyName("total")]
        public long Total { get; set; }
    }
}