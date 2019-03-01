namespace Api.Models.Slack.Blocks
{
    public class SectionBlock
    {
        public string Type => "section";

        public TextBlock Text { get; set; }

        public AccessoryBlock Accessory { get; set; }
    }
}