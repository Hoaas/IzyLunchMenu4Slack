using Newtonsoft.Json;

namespace Api.Models.Slack.Blocks
{
    public interface IAltTextBlock
    {
        [JsonProperty("alt_text")]
        string AltText { get; set; }
    }
}