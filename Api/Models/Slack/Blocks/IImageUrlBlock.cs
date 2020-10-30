using System.Text.Json.Serialization;

namespace Api.Models.Slack.Blocks
{
    public interface IImageUrlBlock
    {
        [JsonPropertyName("image_url")]
        string ImageUrl { get; set; }
    }
}