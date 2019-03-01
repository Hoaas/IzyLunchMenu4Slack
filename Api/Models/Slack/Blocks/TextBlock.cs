namespace Api.Models.Slack.Blocks
{
    public class TextBlock : ITypeBlock
    {
        public string Type => "mrkdwn";

        public string Text { get; set; }
    }
}