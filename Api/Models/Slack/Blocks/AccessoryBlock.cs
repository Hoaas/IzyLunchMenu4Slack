using System.Text.Json.Serialization;

namespace Api.Models.Slack.Blocks
{
    public class AccessoryBlock : ITypeBlock, IImageUrlBlock, IAltTextBlock
    {
        [JsonPropertyName("type")]
        public string Type => "image";
        
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("alt_text")]
        public string AltText { get; set; }
    }
}