using Api.Models.Workplace.WorkplaceSubClasses;
using Newtonsoft.Json;

namespace Api.Models.Workplace
{
    public class WorkplaceResponse
    {
        [JsonProperty("body")]
        public Body Body { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }
    }
}
