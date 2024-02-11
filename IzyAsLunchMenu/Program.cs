using IzyAsLunchMenu;
using IzyAsLunchMenu.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

builder.Services.AddTransient<IzyAsService>();
builder.Services.AddTransient<SlackService>();
builder.Services.AddTransient<BingImageService>();
builder.Services.AddTransient<AiService>();

builder.Services.Configure<Config>(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/kantine", async (IzyAsService izyAsService) => await izyAsService.GetMenu())
    .WithName("kantine")
    .WithOpenApi();

app.MapGet("/kantine/slackwebhook", async (
        string url,
        IzyAsService izyAsService,
        SlackService slackService,
        BingImageService imageService,
        AiService aiService) =>
    {
        var menu = await izyAsService.GetMenu();

        var today = DateTime.Today.AddDays(-5).Date.ToString("yyyy-MM-dd");
        var todaysDishes = menu[today];
        
        if (todaysDishes.Count == 0)
        {
            return TypedResults.Ok("No menu for today.");
        }

        var todaysDishesWithImage = new List<IzyToSlackDishDto>();
        foreach (var dish in todaysDishes)
        {
            var aiOptimizedSearchTerm = await aiService.OptimizeSearchTerm(dish.name);
            var (imageUrl, searchTerm) = await imageService.GetImageUrl(aiOptimizedSearchTerm ?? dish.name);
            
            todaysDishesWithImage.Add(new IzyToSlackDishDto
            {
                Dishes = dish,
                ImageUrl = imageUrl,
                ImageQueryUsedForUrl = searchTerm
            });
        }


        var todaysmenu = new Dictionary<string, List<IzyToSlackDishDto>>
        {
            { today, todaysDishesWithImage }
        };

        await slackService.SendToSlack(todaysmenu, url);

        return TypedResults.Ok("All ok!");
    })
    .WithName("kantine/post-to-url")
    .WithOpenApi();

app.Run();