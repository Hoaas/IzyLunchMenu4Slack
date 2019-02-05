using System;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Models.Workplace;

namespace Api
{
    public class HelsedirMenuFetcher : IHelsedirMenuFetcher
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string MenuUrl = "https://workplace.izy.as/api/menu-items/featured";

        private WorkplaceResponse _responseCache;
        private DateTime _cacheTime = DateTime.MinValue;

        public HelsedirMenuFetcher(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<WorkplaceResponse> ReadMenu()
        {
            if (_cacheTime.AddMinutes(15) > DateTime.UtcNow) return _responseCache;

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(MenuUrl);

            var parseResponse = await response.Content.ReadAsAsync<WorkplaceResponse>();

            _responseCache = parseResponse;
            _cacheTime = DateTime.UtcNow;

            return _responseCache;

        }
    }
}
