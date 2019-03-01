namespace Api.Models.Slack.Blocks
{
    public class AccessoryBlock : ITypeBlock, IImageUrlBlock, IAltTextBlock
    {
        public string Type => "image";

        public string ImageUrl { get; set; }

        public string AltText { get; set; }
    }
}