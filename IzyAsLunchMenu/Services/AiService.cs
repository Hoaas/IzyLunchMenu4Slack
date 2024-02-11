using Microsoft.Extensions.Options;
using OpenAI_API;

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
        var api = new OpenAIAPI(_apiKey);
        
        var result = await api.Chat.CreateChatCompletionAsync($"Create a better search term, in english, optimized for Bing Image Search for this dish: '{dishName}', but try to avoid using quotation marks.");

        return result.Choices[0].Message.TextContent;
    }
}