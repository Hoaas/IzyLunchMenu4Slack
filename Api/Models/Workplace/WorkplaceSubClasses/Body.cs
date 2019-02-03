using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Api.Models.Workplace.WorkplaceSubClasses
{
    public class Body
    {
        [JsonProperty("current_page")]
        public long CurrentPage { get; set; }

        [JsonProperty("data")]
        public List<Datum> Data { get; set; }

        [JsonProperty("first_page_url")]
        public Uri FirstPageUrl { get; set; }

        [JsonProperty("from")]
        public long From { get; set; }

        [JsonProperty("last_page")]
        public long LastPage { get; set; }

        [JsonProperty("last_page_url")]
        public Uri LastPageUrl { get; set; }

        [JsonProperty("next_page_url")]
        public dynamic NextPageUrl { get; set; }

        [JsonProperty("path")]
        public Uri Path { get; set; }

        [JsonProperty("per_page")]
        public long PerPage { get; set; }

        [JsonProperty("prev_page_url")]
        public dynamic PrevPageUrl { get; set; }

        [JsonProperty("to")]
        public long To { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }
}