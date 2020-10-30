using System.Text.Json.Serialization;

namespace Api.Models.Slack.Blocks
{
    public class AccessoryBlock : ITypeBlock, IImageUrlBlock, IAltTextBlock
    {
        [JsonPropertyName("type")]
        public string Type => "image";
        
        [JsonPropertyName("imageurl")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("alttext")]
        public string AltText { get; set; }
    }
}