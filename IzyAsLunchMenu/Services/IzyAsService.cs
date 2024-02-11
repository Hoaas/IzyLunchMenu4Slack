using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Options;

namespace IzyAsLunchMenu.Services;

public class IzyAsService
{
    private readonly HttpClient _client;
    private readonly Config _config;
    private const string BaseUrl = "https://api.izy.as";
    
    public IzyAsService(IHttpClientFactory clientFactory, IOptions<Config> options)
    {
        _client = clientFactory.CreateClient();
        _client.BaseAddress = new Uri(BaseUrl);
        _config = options.Value;
    }

    public async Task<Dictionary<string, List<Dishes>>> GetMenu()
    {
        var token = await GetToken();

        _client.DefaultRequestHeaders.Authorization ??= new AuthenticationHeaderValue("Bearer", token);

        var buildings = await GetListOfBuildings();

        // Or like this if you need to get a specific building (and know the name)
        // var building = buildings.Where(b => b.Name == "Workplace Oo").First();
        var building = buildings.First();
        
        var canteens = await GetListOfCanteens(building.Id);

        var canteen = canteens.First();
        var menu = await GetMenuForCanteen(building.Id, canteen.Id);

        if (menu.body.week != ISOWeek.GetWeekOfYear(DateTime.Now))
        {
            throw new Exception("No menu for this week yet.");
        }
        
        var dict = new Dictionary<string, List<Dishes>>
        {
            [menu.body.Monday.date] = menu.body.Monday.menus.FirstOrDefault()?.dishes ?? [],
            [menu.body.Tuesday.date] = menu.body.Tuesday.menus.FirstOrDefault()?.dishes ?? [],
            [menu.body.Wednesday.date] = menu.body.Wednesday.menus.FirstOrDefault()?.dishes ?? [],
            [menu.body.Thursday.date] = menu.body.Thursday.menus.FirstOrDefault()?.dishes ?? [],
            [menu.body.Friday.date] = menu.body.Friday.menus.FirstOrDefault()?.dishes ?? [],
            [menu.body.Saturday.date] = menu.body.Saturday.menus.FirstOrDefault()?.dishes ?? [],
            [menu.body.Sunday.date] = menu.body.Sunday.menus.FirstOrDefault()?.dishes ?? []
        };

        return dict;
    }

    private async Task<string> GetToken()
    {
        var response = await _client.PostAsJsonAsync(
            "/api/auth/authenticate",
            new
            {
                username = _config.IzyAsUsername,
                password = _config.IzyAsPassword
            });

        if (!response.IsSuccessStatusCode) throw new Exception("Failed to get token");
        
        var bearerToken = response.Headers.GetValues("access-token").FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(bearerToken)) throw new Exception("Failed to get token - but got 200 Ok ...");
        
        return bearerToken;
    }
    
    private async Task<IEnumerable<(int Id, string Name)>> GetListOfBuildings()
    {
        var response = await _client.GetFromJsonAsync<JsonNode>("/api/user-buildings");
        
        var buildings = ((JsonArray)response["body"]["buildings"])
            .Select(x => (Id: x["id"].GetValue<int>(), Name: x["name"].GetValue<string>()));
        
        return buildings;
    }

    private async Task<IEnumerable<(int Id, string Name)>> GetListOfCanteens(int buildingId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/canteens");
        
        // To izy developers: wtf does the error say "add Building Id to the request headers" and "empty_building_id" when the header is on neither of those formats⁉️
        request.Headers.Add("building-id", buildingId.ToString());

        var response = await _client.SendAsync(request);
        var json = await response.Content.ReadFromJsonAsync<JsonNode>();
        
        var canteens = ((JsonArray)json["body"]["data"]).Select(x => (Id: x["id"].GetValue<int>(), Name: x["name"].GetValue<string>()));
        
        return canteens;
    }
    
    private async Task<GetDishesResponse> GetMenuForCanteen(int buildingId, int canteenId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/dishes");
        // var request = new HttpRequestMessage(HttpMethod.Get, "/api/dishes?canteen_id=" + canteenId);

        request.Headers.Add("building-id", buildingId.ToString());

        var response = await _client.SendAsync(request);

        return await response.Content.ReadFromJsonAsync<GetDishesResponse>();
    }
}