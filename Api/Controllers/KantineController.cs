using System.Collections.Generic;
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

        [Route("/")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await CreateSlackMessage());
        }

        [Route("slack")]
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
            var menu = await _menuService.FetchMenu();

            var message = new SlackMessage
            {
                attachments = new List<SlackAttachment>()
            };

            foreach (var day in menu.Keys)
            {
                foreach (var menuItem in menu[day])
                {
                    message.attachments.Add(new SlackAttachment
                    {
                        text = menuItem
                    });
                }

            }
            return message;
        }
    }
}