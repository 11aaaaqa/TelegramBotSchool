using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotSchool.Database;
using TelegramBotSchool.Services;

namespace TelegramBotSchool.Commands
{
    public class ShowAllCommand : BaseCommand
    {
        private readonly ApplicationDbContext context;
        private readonly TelegramBotClient client;
        private readonly IGetInlineKeyboardMarkup markup;
        public ShowAllCommand(ApplicationDbContext context, Bot bot, IGetInlineKeyboardMarkup markup)
        {
            this.context = context;
            client = bot.GetBot();
            this.markup = markup;
        }
        public override string Name => "showAll";
        public override async Task ExecuteAsync(Update update)
        {
            var list = context.Reminders.ToList();
        }
    }
}
