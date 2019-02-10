using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Config;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Rest;

namespace Api.ImageSearch
{
    public class ImageSearcher : IImageSearcher
    {
        private readonly AzureCognitiveConfig _config;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ImageSearcher> _logger;
        private readonly Random _random = new Random();


        public ImageSearcher(
            IOptions<AzureCognitiveConfig> azureCognitiveOptions,
            IMemoryCache memoryCachecache,
            ILoggerFactory loggerFactory)
        {
            _cache = memoryCachecache;
            _logger = loggerFactory.CreateLogger<ImageSearcher>();
            _config = azureCognitiveOptions.Value;
        }

        public async Task<string> SearchForMeal(string meal)
        {
            var searchTerm = meal;

            var mainMeal = meal.Split("med", 2, StringSplitOptions.RemoveEmptyEntries);
            if (mainMeal.Length == 2)
            {
                searchTerm = mainMeal.First();
            }

            while (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var urls = await Search(searchTerm);
                if (urls.Any()) return PickRandom(urls);

                searchTerm = RemoveLastWord(searchTerm);
            }

            return null;
        }

        private string PickRandom(IReadOnlyList<string> strings)
        {
            var hits = strings.Count;
            {
                return strings[_random.Next(0, hits)];
            }
        }

        private async Task<List<string>> Search(string searchTerm)
        {
            var cacheKey = $"search-{searchTerm}";
            if (_cache.TryGetValue(cacheKey, out List<string> cacheEntry)) return cacheEntry;

            var client = new ImageSearchClient(new ApiKeyServiceClientCredentials(_config.FaceApi))
            {
                Endpoint = _config.Endpoint
            };

            HttpOperationResponse<Images> results;
            try
            {
                _logger.LogInformation($"Image search for '{searchTerm}'.");
                results = await client.Images.SearchWithHttpMessagesAsync(
                    searchTerm,
                    safeSearch: "Moderate",
                    countryCode: "no-no"
                    );
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Image search failed.");
                return null;
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
            return index == -1 ? null : searchTerm.Substring(0, index).Trim();
        }
    }
}
