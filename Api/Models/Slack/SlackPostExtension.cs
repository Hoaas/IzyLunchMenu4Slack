namespace Api.Models.Slack
{
    public static class SlackPostExtension
    {
        public static bool IsCommand(this SlackPost post, string command)
        {
            if (post == null) return false;
            return !string.IsNullOrWhiteSpace(post?.text) && post.text.ToLower().Contains(command.ToLower());
        }
    }
}
