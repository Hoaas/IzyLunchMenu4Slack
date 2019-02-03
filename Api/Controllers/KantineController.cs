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

            if (post?.command == null || !post.command.Contains("announce"))
            {
                message.response_type = "ephemeral";
            }

            return Ok(message);
        }

        private async Task<SlackMessage> CreateSlackMessage()
        {
            var specificDay = DateTime.Now.ToString("dddd", CultureInfo.GetCultureInfo("nb-NO"));

            var menu = await _menuService.FetchMenu();

            var message = new SlackMessage
            {
                text = specificDay
            };

            message.attachments.AddRange(CreateSlackAttachment(menu[specificDay]));

            return message;
        }

        private static IEnumerable<SlackAttachment> CreateSlackAttachment(IEnumerable<string> meals)
        {
            return meals.Select(meal => new SlackAttachment
            {
                text = meal
            });
        }
    }
}