namespace Api.Models.Slack.Blocks
{
    public class SectionBlock : ITypeBlock
    {
        public string Type => "section";

        public TextBlock Text { get; set; }

        public AccessoryBlock Accessory { get; set; }
    }
}