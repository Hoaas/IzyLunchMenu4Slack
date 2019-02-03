using Newtonsoft.Json;

namespace Api.Models.Workplace.WorkplaceSubClasses
{
    public class Extra
    {
        [JsonProperty("ingredients")]
        public string Ingredients { get; set; }
    }
}