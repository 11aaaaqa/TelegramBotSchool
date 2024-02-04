using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotSchool.Database;
using TelegramBotSchool.Services;

namespace TelegramBotSchool.Commands
{
    public class ChangeDifferenceCommand : BaseCommand
    {
        private readonly TelegramBotClient client;
        private readonly ApplicationDbContext context;
        private readonly IGetInlineKeyboardMarkup markup;
        public ChangeDifferenceCommand(Bot bot, ApplicationDbContext context, IGetInlineKeyboardMarkup markup)
        {
            this.context = context;
            client = bot.GetBot();
            this.markup = markup;
        }
        public override string Name => "changeDifference";
        public override async Task ExecuteAsync(Update update)
        {
            var toBack = markup.GetToBack();
            var message = update.CallbackQuery.Message;
            DateTime utc = DateTime.UtcNow;

            var diff = context.Users.Single(x => x.ChatId == message.Chat.Id.ToString()).Difference;
            var user = await context.Users.FindAsync(message.Chat.Id.ToString());
            user.IsDifferenceSet = false;

            await context.SaveChangesAsync();

            await client.EditMessageTextAsync(message.Chat.Id, message.MessageId,
                "Пожалуйста, введите вашу разницу с всемирным координированным временем (Например: 1, -1, 0)\n\n" +
                $"Текущее всемирное координированное время: {utc}\n\nВаша текущая разница во времени: {diff}", replyMarkup: toBack);

        }
    }
}
