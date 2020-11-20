using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Models.Workplace;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class HelsedirMenuFetcher : IHelsedirMenuFetcher
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        private readonly ILogger<HelsedirMenuFetcher> _logger;

        private const string MenuUrl = "https://workplace.izy.as/api/menu-items/featured";
        private const string MenuCacheKey = "MenuCacheKey";

        public HelsedirMenuFetcher(
            IHttpClientFactory httpClientFactory,
            IMemoryCache cache,
            ILoggerFactory loggerFactory)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
            _logger = loggerFactory.CreateLogger<HelsedirMenuFetcher>();
        }

        public async Task<WorkplaceResponse> ReadMenu()
        {
            if (_cache.TryGetValue(MenuCacheKey, out WorkplaceResponse cacheEntry)) return cacheEntry;

            var client = _httpClientFactory.CreateClient();
            try
            {
                _logger.LogInformation("Attempting to fetch menu.");
                var response = await client.GetAsync(MenuUrl);
                var stream = await response.Content.ReadAsStreamAsync();
                cacheEntry = await JsonSerializer.DeserializeAsync<WorkplaceResponse>(stream);
            }
            catch (Exception)
            {
                throw new WorkplaceNotWorkingException();
            }

            _cache.Set(MenuCacheKey, cacheEntry, DateTime.Now.AddMinutes(15));

            return cacheEntry;

        }
    }
}
