using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IzyAsLunchMenu.Services;

public class SlackService
{
    private readonly HttpClient _client;

    public SlackService(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient();
    }
    
    public async Task SendAiFormattedMenuToSlack(string formattedMenu, string url)
    {
        var message = new { text = "*Ukens meny formatert av ChatGPT*\n" + formattedMenu };
        await _client.PostAsJsonAsync(url, message);
    }

    public async Task SendToSlack(Dictionary<string, List<IzyToSlackDishDto>> menu, string url)
    {
        if (menu.Values.Count == 0 || menu.Values.First().Count == 0) return;

        var message = new
        {
            blocks = CreateSlackBlocks(menu),
            response_type = "ephemeral" // or "in_channel"
        };
        
        var result = await _client.PostAsJsonAsync(url, message, new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        if (!result.IsSuccessStatusCode)
        {
            var error = await result.Content.ReadAsStringAsync();
            throw new Exception("Failed to send message to Slack: " + error);
        }
    }

    private List<object> CreateSlackBlocks(Dictionary<string, List<IzyToSlackDishDto>> menus)
    {
        var blocks = new List<object>();

        foreach (var (date, dishesAndUrls) in menus)
        {
            if (dishesAndUrls.Count == 0) continue;
 
            var day = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var formattedDay = day.ToString("dddd d. MMMM", new CultureInfo("nb-NO"));
            blocks.Add(new
            {
                type = "header",
                text = new
                {
                    type = "plain_text",
                    text = $"Meny for {formattedDay}",
                    emoji = true
                }               
            });
            
            foreach (var dishDto in dishesAndUrls)
            {
                if (string.IsNullOrWhiteSpace(dishDto.ImageUrl))
                {
                    blocks.Add(new
                    {
                        type = "section",
                        text = new
                        {
                            type = "mrkdwn",
                            text = $"*{dishDto.Dishes.name}*"
                        }
                    });
                }
                else
                {
                    blocks.Add(new
                    {
                        type = "section",
                        text = new
                        {
                            type = "mrkdwn",
                            text = $"*{dishDto.Dishes.name}*"
                        },
                        accessory = new
                        {
                            type = "image",
                            alt_text = $"Tilfeldig resultat fra Bing Image Search etter '{dishDto.ImageQueryUsedForUrl}'",
                            image_url = dishDto.ImageUrl
                        }
                    });
                }
            }
        }

        return blocks;
    }
}