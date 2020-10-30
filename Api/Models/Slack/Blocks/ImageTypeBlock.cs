using System.Text.Json.Serialization;

namespace Api.Models.Slack.Blocks
{
    public class ImageTypeBlock : ITypeBlock, IImageUrlBlock, IAltTextBlock
    {
        [JsonPropertyName("type")]
        public string Type => "image";

        [JsonPropertyName("title")]
        public TextBlock Title { get; set; }

        [JsonPropertyName("imageurl")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("alttext")]
        public string AltText { get; set; }
    }
}