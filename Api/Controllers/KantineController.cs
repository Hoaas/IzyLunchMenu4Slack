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
            return Ok(await CreateSlackMessage());
        }

        [Route("kantine/slack")]
        [HttpPost]
        public async Task<IActionResult> SlackDirectResponse([FromForm] SlackPost post)
        {
            var message = await CreateSlackMessage();

            if (post?.text == null || !post.text.Contains("announce"))
            {
                message.response_type = "ephemeral";
            }

            return Ok(message);
        }

        private async Task<SlackMessage> CreateSlackMessage()
        {

            var menu = await _menuService.FetchMenu();

            var specificDay = DateTime.Now.ToString("dddd", CultureInfo.GetCultureInfo("nb-NO"));

            var tries = 0;

            List<string> meals;
            while (!menu.TryGetValue(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(specificDay), out meals))
            {
                tries++;

                specificDay = DateTime.Now.AddDays(tries).ToString("dddd", CultureInfo.GetCultureInfo("nb-NO"));
                if (tries <= 7) continue;

                return new SlackMessage
                {
                    text = "Ingen måltider funnet :("
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
