using Newtonsoft.Json;

namespace Api.Models.Slack.Blocks
{
    public interface IImageUrlBlock
    {
        [JsonProperty("image_url")]
        string ImageUrl { get; set; }
    }
}