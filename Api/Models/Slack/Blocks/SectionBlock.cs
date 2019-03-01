using Newtonsoft.Json;

namespace Api.Models.Slack.Blocks
{
    public class SectionBlock : ITypeBlock
    {
        public string Type => "section";

        public TextBlock Text { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AccessoryBlock Accessory { get; set; }
    }
}