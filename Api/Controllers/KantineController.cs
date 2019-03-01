using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Api.ImageSearch;
using Api.Models.Slack;
using Api.Models.Slack.Blocks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [ApiController]
    public class KantineController : ControllerBase
    {
        private readonly IHelsedirMenuService _menuService;
        private readonly IImageSearcher _imageSearcher;
        private readonly IHttpClientFactory _httpClientFactory;

        public KantineController(
            IHelsedirMenuService menuService,
            IImageSearcher imageSearcher,
            IHttpClientFactory httpClientFactory)
        {
            _menuService = menuService;
            _imageSearcher = imageSearcher;
            _httpClientFactory = httpClientFactory;
        }

        [Route("kantine")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await CreateSlackMessage(false));
        }

        //[Route("kantine/image")]
        //[HttpGet]
        //public async Task<IActionResult> Get(string meal)
        //{
        //    var url = await _imageSearcher.SearchForMeal(meal);

        //    if (url == null) return NotFound("No image found :'(");

        //    return Ok(url);
        //}

        [Route("kantine/post-to-url")]
        [HttpGet]
        public async Task<IActionResult> PostToUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || !url.StartsWith("https://hooks.slack.com/services/"))
            {
                return BadRequest("Requires URL. And must start with https://hooks.slack.com/services/.");
            }

            var message = await CreateSlackMessage(allInOneNastyBlob: false);
            message.response_type = "ephemeral";

            var client = _httpClientFactory.CreateClient();

            var json = JsonConvert.SerializeObject(message);

            await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));

            return Ok();
        }

        [Route("kantine/slack")]
        [HttpPost]
        public async Task<IActionResult> SlackDirectResponse([FromForm] SlackPost post)
        {
            SlackMessage message = null;
            if (post?.text != null)
            {
                if (post.IsCommand("help") || post.IsCommand("hjelp"))
                {
                    message = new SlackMessage
                    {
                        text = "Kommandoer:\n" +
                               "announce - Gir output til hele kanalen\n" +
                               "all - Viser menyen for hele uka\n" +
                               "help - Denne hjelpen.\n" +
                               "\n" +
                               "https://github.com/Hoaas/Vitaminveien4Menu4Slack"
                    };
                }
                else if (post.IsCommand("all") || post.IsCommand("alt"))
                {
                    message = await CreateSlackMessage(allInOneNastyBlob: true);
                }
            }

            if (message == null)
            {
                message = await CreateSlackMessage(allInOneNastyBlob: false);
            }

            if (!post.IsCommand("announce"))
            {
                message.response_type = "ephemeral";
            }

            return Ok(message);
        }

        private async Task<SlackMessage> CreateSlackMessage(bool allInOneNastyBlob)
        {
            if (allInOneNastyBlob)
            {
                try
                {
                    var allMenu = await _menuService.FetchEntireMenuAsText();
                    return new SlackMessage { text = allMenu };
                }
                catch (WorkplaceNotWorkingException)
                {
                    return new SlackMessage { text = "https://workplace.izy.as/ er nede?" };
                }
            }
            try
            {
                var menu = await _menuService.FetchMenu();
                return await CreateMessageForSpesificDay(menu);
            }
            catch (WorkplaceNotWorkingException)
            {
                return new SlackMessage { text = "https://workplace.izy.as/ er nede?" };

            }
        }

        private async Task<SlackMessage> CreateMessageForSpesificDay(Dictionary<string, List<string>> menu)
        {
            var today = DateTime.Now.ToString("dddd", CultureInfo.GetCultureInfo("nb-NO"));

            var specificDay = today;

            var tries = 0;
            List<string> meals;
            while (!menu.TryGetValue(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(specificDay), out meals))
            {
                tries++;

                specificDay = DateTime.Now.AddDays(tries).ToString("dddd", CultureInfo.GetCultureInfo("nb-NO"));
                if (tries <= 7) continue;

                return new SlackMessage
                {
                    text = $"Finner ikke meny for {today} (eller andre dager for den saksskyld)"
                };
            }

            var message = new SlackMessage
            {
                text = $"Meny for {specificDay}",
                attachments = await CreateSlackAttachment(meals),
            };

            return message;
        }

        private async Task<List<SectionBlock>> CreateSlackAttachment(IEnumerable<string> meals)
        {
            var blocks = new List<SectionBlock>();
            foreach (var meal in meals)
            {
                var block = new SectionBlock
                {
                    Text = new TextBlock(),
                    Accessory = new AccessoryBlock()
                };
                var dishName = meal.Split(":")[1].Trim();
                if (!string.IsNullOrWhiteSpace(dishName))
                {
                    block.Text.Text = $"*{dishName}*";
                    var url = await _imageSearcher.SearchForMeal(dishName);
                    if (url != null)
                    {
                        block.Accessory.AltText = dishName;
                        block.Accessory.ImageUrl = url;
                    }
                }
                blocks.Add(block);
            }

            return blocks;
        }
    }
}
