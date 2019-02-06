using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Api.ImageSearch;
using Api.Models.Slack;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    public class KantineController : ControllerBase
    {
        private readonly IHelsedirMenuService _menuService;

        private readonly IImageSearcher _imageSearcher;

        public KantineController(
            IHelsedirMenuService menuService,
            IImageSearcher imageSearcher)
        {
            _menuService = menuService;
            _imageSearcher = imageSearcher;
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
                    message = await CreateSlackMessage(allInOneNastyBlob: false);
                }
            }
            else
            {
                message = await CreateSlackMessage(allInOneNastyBlob: true);
            }

            if (message == null)
            {
                message = new SlackMessage
                {
                    text = "Woops. Noe gikk fryktelig galt."
                };
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

        private async Task<List<SlackAttachment>> CreateSlackAttachment(IEnumerable<string> meals)
        {
            var attachments = new List<SlackAttachment>();
            foreach (var meal in meals)
            {
                var att = new SlackAttachment { text = meal };
                if (meal.StartsWith("Varmrett"))
                {
                    var hotdish = meal.Replace("Varmrett", string.Empty).Replace(":", string.Empty).Trim();
                    var url = await _imageSearcher.SearchForMeal(hotdish);
                    if (url != null)
                    {
                        att.image_url = url;
                    }
                }
                attachments.Add(att);
            }
            return attachments;
        }
    }
}
