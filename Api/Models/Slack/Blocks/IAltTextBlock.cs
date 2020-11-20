using System.Text.Json.Serialization;

namespace Api.Models.Slack.Blocks
{
    public interface IAltTextBlock
    {
        [JsonPropertyName("alt_text")]
        string AltText { get; set; }
    }
}