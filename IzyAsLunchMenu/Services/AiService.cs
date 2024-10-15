using System.ClientModel;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;

namespace IzyAsLunchMenu.Services;

public class AiService
{
    private readonly string _apiKey;
    private readonly string _endpoint;

    public AiService(IOptions<Config> options)
    {
        _apiKey = options.Value.OpenAiSubscriptionKey;
        _endpoint = options.Value.OpenAiEndpoint;
    }
    
    public async Task<string?> OptimizeSearchTerm(string dishName)
    {
        return await GetResponseFromSingleMessage(
            $"Create a better search term, in english, optimized for Bing Image Search for this dish: '{dishName}'. " +
            $"Try to avoid using quotation marks.");
    }

    public async Task<string> GetFormattedMenu(string menu)
    {
        return await GetResponseFromSingleMessage(
            "Extract each days menu from this data, and format the menu for the days available. " +
            "The message will be sent to Slack, so use Slack-compatible formatting aka. 'mrkdwn'. " +
            "_italic_ will produce italicized text " +
            "*bold* will produce bold text. " +
            "Add emojis (unicode, not :format:) that fits for each dish. " +
            "Only return the formatted menu, no additional text. " +
            "Oh, and respond in norwegian. " +
            menu);
    }

    private async Task<string> GetResponseFromSingleMessage(string message)
    {
        var aiClient = new AzureOpenAIClient(new Uri(_endpoint), new ApiKeyCredential(_apiKey));
        var client = aiClient.GetChatClient("gpt-4o");
        var result = await client.CompleteChatAsync(message);

        return result.Value.Content[0].Text;
    }
}
