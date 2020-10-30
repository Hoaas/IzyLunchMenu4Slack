using System.Text.Json.Serialization;

namespace Api.Models.Slack.Blocks
{
    public class TextBlock : ITypeBlock
    {
        [JsonPropertyName("type")]
        public string Type => "mrkdwn";

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}