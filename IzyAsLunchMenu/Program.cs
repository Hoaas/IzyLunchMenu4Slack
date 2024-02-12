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
    .WithName("/kantine")
    .WithOpenApi();

app.MapGet("/kantine/ukesmeny", async (
        IzyAsService izyAsService,
        AiService aiService) =>
    {
        var menu = await izyAsService.GetRawResponse();
        var formattedMenu = await aiService.GetFormattedMenu(menu);
        return formattedMenu;
    })
    .WithName("/kantine/ukesmeny")
    .WithOpenApi();

app.MapGet("/kantine/ukesmeny/slackwebhook", async (
        string url,
        IzyAsService izyAsService,
        SlackService slackService,
        AiService aiService) =>
    {
        var menu = await izyAsService.GetRawResponse();
        var formattedMenu = await aiService.GetFormattedMenu(menu);

        await slackService.SendAiFormattedMenuToSlack(formattedMenu, url);
        
        return TypedResults.Ok("👍");
    })
    .WithName("/kantine/ukesmeny/slackwebhook")
    .WithOpenApi();

app.MapGet("/kantine/slackwebhook", async (
        string url,
        IzyAsService izyAsService,
        SlackService slackService,
        BingImageService imageService,
        AiService aiService) =>
    {
        var menu = await izyAsService.GetMenu();

        var today = DateTime.Today.Date.ToString("yyyy-MM-dd");
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
    .WithName("/kantine/slackwebhook")
    .WithOpenApi();

app.Run();