using System.Collections.Generic;

namespace Api.Models.Slack
{
    public class SlackMessage
    {
        // If this is not set the response is only visible to the one that triggered the command.
        // The other option is ephemeral. That will have the same effect as null/empty.
        public string response_type { get; set; } = "in_channel";
        public string text { get; set; }
        //public List<Dictionary<string,string>> attachments { get; set; }
        public List<SlackAttachment> attachments { get; set; }
        //public string username { get; set; }
        //public string icon_emoji { get; set; }
        //public string icon_url { get; set; }
    }
}
