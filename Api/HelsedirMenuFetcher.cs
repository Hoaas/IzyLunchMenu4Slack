using System.Net.Http;
using System.Threading.Tasks;
using Api.Models.Workplace;

namespace Api
{
    public class HelsedirMenuFetcher : IHelsedirMenuFetcher
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string MenuUrl = "https://workplace.izy.as/api/menu-items/featured";

        public HelsedirMenuFetcher(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<WorkplaceResponse> ReadMenu()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(MenuUrl);

            var parseResponse = await response.Content.ReadAsAsync<WorkplaceResponse>();

            return parseResponse;

        }
    }

    public interface IHelsedirMenuFetcher
    {
        Task<WorkplaceResponse> ReadMenu();
    }
}
