using System.Text.Json.Serialization;

namespace Api.Models.Workplace.WorkplaceSubClasses
{
    public class Extra
    {
        [JsonPropertyName("ingredients")]
        public string Ingredients { get; set; }
    }
}