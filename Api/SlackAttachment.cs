namespace Api
{
    public class SlackAttachment
    {
        public string fallback { get; set; }
        public string color { get; set; }
        //public string pretext { get; set; }
        //public string author_name { get; set; }
        //public string author_link { get; set; }
        //public string author_icon { get; set; }
        public string title { get; set; }
        public string title_link { get; set; }
        //[JsonProperty(PropertyName = "text")]
        public string text { get; set; }
        //public SlackAttachmentField fields { get; set; }
        //public string image_url { get; set; }
        public string thumb_url { get; set; }
    }

    //public class SlackAttachmentField
    //{
    //    public string title { get; set; }
    //    public string value { get; set; }
    //    // public bool short { get; set; }
    //}
}
