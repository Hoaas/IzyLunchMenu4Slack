namespace Api.Models.Slack.Blocks
{
    public class ImageTypeBlock : ITypeBlock, IImageUrlBlock, IAltTextBlock
    {
        public string Type => "image";

        public TextBlock Title { get; set; }

        public string ImageUrl { get; set; }

        public string AltText { get; set; }
    }
}