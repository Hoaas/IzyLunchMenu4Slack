using System.Text.Json.Serialization;
using Api.Models.Workplace.WorkplaceSubClasses;

namespace Api.Models.Workplace
{
    public class WorkplaceResponse
    {
        [JsonPropertyName("body")]
        public Body Body { get; set; }

        [JsonPropertyName("status")]
        public Status Status { get; set; }
    }
}
