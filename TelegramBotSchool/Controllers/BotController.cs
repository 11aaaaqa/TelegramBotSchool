using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotSchool.Commands;

namespace TelegramBotSchool.Controllers
{
    [Route("api/bot")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly ICommandExecutor executor;

        public BotController(ICommandExecutor executor)
        {
            this.executor = executor;
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Update update)
        {
            if (update.Message?.Chat == null && update.CallbackQuery == null)
            {
                return Ok();
            }

            //if (update.Message.Text != null)
            //{
            await executor.Execute(update);
            //}

            return Ok();
        }
    }
}
