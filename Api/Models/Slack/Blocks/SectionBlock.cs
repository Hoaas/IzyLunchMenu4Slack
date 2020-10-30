using System.Text.Json.Serialization;

namespace Api.Models.Slack.Blocks
{
    public class SectionBlock : ITypeBlock
    {
        public string Type => "section";

        [JsonPropertyName("text")]
        public TextBlock Text { get; set; }

        [JsonPropertyName("accessory")]
        public AccessoryBlock Accessory { get; set; }
    }
}