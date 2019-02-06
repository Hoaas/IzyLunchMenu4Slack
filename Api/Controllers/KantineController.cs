using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Api.Models.Slack;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    public class KantineController : ControllerBase
    {
        private readonly IHelsedirMenuService _menuService;

        public KantineController(IHelsedirMenuService menuService)
        {
            _menuService = menuService;
        }

        [Route("kantine")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await CreateSlackMessage(false));
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
                               "help - Denne hjelpen\n"
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
                var allMenu = await _menuService.FetchEntireMenuAsText();
                return new SlackMessage { text = allMenu };
            }

            var menu = await _menuService.FetchMenu();
            return CreateMessageForSpesificDay(menu);
        }

        private static SlackMessage CreateMessageForSpesificDay(Dictionary<string, List<string>> menu)
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
                attachments = CreateSlackAttachment(meals),
            };
            return message;
        }

        private static List<SlackAttachment> CreateSlackAttachment(IEnumerable<string> meals)
        {
            return meals.Select(meal => new SlackAttachment
            {
                text = meal
            })
            .ToList();
        }
    }
}
