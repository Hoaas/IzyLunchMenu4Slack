namespace Api
{
    public class SlackPost
    {
        //token=cyZ077zYNx5Z88OCX7VrNNT1
        //team_id = T0EA1QH18
        //team_domain=vismaconsultingnorway
        //channel_id = D0EA63QTY
        //channel_name=directmessage
        //user_id = U0EA63PNJ
        //user_name=terje.hoaas
        //command=%2Ftemp
        //text =
        //response_url = https % 3A%2F%2Fhooks.slack.com%2Fcommands%2FT0EA1QH18%2F24194046481%2FITS1YxH0vBXOep4WbwVlO3bS
        public string token { get; set; }
        public string team_id { get; set; } //TeamId
        public string team_domain { get; set; }
        public string channel_id { get; set; }
        public string channel_name { get; set; }
        public string user_id { get; set; }
        public string command { get; set; }
        public string user_name { get; set; }
        public string text { get; set; }
        public string response_url { get; set; }
    }
}
