using Microsoft.Bing.ImageSearch;
using Microsoft.Bing.ImageSearch.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Rest;

namespace IzyAsLunchMenu.Services;

public class BingImageService
{
    private readonly IMemoryCache _cache;
    private readonly string _apiKey;


    public BingImageService(
        IOptions<Config> options,
        IMemoryCache cache)
    {
        _cache = cache;
        _apiKey = options.Value.BingSearchSubscriptionKey;
    }
    
    public async Task<(string? Url, string? SearchTerm)> GetImageUrl(string meal)
    {
        var searchTerm = meal;

        while (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var urls = await Search(searchTerm);
            if (urls.Count != 0) return (PickRandom(urls), searchTerm);

            searchTerm = RemoveLastWord(searchTerm);
        }

        return (null, null);
    }

    private string PickRandom(IReadOnlyList<string> strings)
    {
        var hits = strings.Count;
        {
            return strings[Random.Shared.Next(0, hits)];
        }
    }

    private async Task<List<string>> Search(string searchTerm)
    {
        var cacheKey = $"search-{searchTerm}";
        if (_cache.TryGetValue(cacheKey, out List<string> cacheEntry)) return cacheEntry;

        var client = new ImageSearchClient(new ApiKeyServiceClientCredentials(_apiKey));

        HttpOperationResponse<Images> results;
        try
        {
            results = await client.Images.SearchWithHttpMessagesAsync(
                searchTerm,
                safeSearch: "Moderate",
                countryCode: "no-no"
            );
        }
        catch (Exception e)
        {
            return [];
        }

        var searchResults = results?.Body?.Value;
        if (searchResults != null)
        {
            cacheEntry = searchResults.Take(10).Select(x => x.ContentUrl).ToList();
        }

        _cache.Set(cacheKey, cacheEntry, DateTime.Now.AddDays(1));

        return cacheEntry;
    }

    private static string RemoveLastWord(string searchTerm)
    {
        var index = searchTerm.LastIndexOf(' ');
        return index == -1
            ? string.Empty
            : searchTerm[..index].Trim();
    }
}