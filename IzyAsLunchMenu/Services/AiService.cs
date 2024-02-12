using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;

namespace IzyAsLunchMenu.Services;

public class AiService
{
    private readonly string _apiKey;

    public AiService(IOptions<Config> options)
    {
        _apiKey = options.Value.OpenAiSubscriptionKey;
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
        var api = new OpenAIClient(_apiKey);

        var result = await api.GetChatCompletionsAsync(new ChatCompletionsOptions
        {
            DeploymentName = "gpt-3.5-turbo",
            Messages =
            {
                new ChatRequestUserMessage(message)
            }
        });

        return result.Value.Choices[0].Message.Content;
    }
}