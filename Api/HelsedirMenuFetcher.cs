using System;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Models.Workplace;
using Microsoft.Extensions.Caching.Memory;

namespace Api
{
    public class HelsedirMenuFetcher : IHelsedirMenuFetcher
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        private const string MenuUrl = "https://workplace.izy.as/api/menu-items/featured";
        private const string MenuCacheKey = "MenuCacheKey";

        public HelsedirMenuFetcher(
            IHttpClientFactory httpClientFactory,
            IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }

        public async Task<WorkplaceResponse> ReadMenu()
        {
            if (_cache.TryGetValue(MenuCacheKey, out WorkplaceResponse cacheEntry)) return cacheEntry;

            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.GetAsync(MenuUrl);
                cacheEntry = await response.Content.ReadAsAsync<WorkplaceResponse>();
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
