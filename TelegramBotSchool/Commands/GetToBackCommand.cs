using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotSchool.Database;
using TelegramBotSchool.Services;

namespace TelegramBotSchool.Commands
{
    public class GetToBackCommand : BaseCommand
    {
        private readonly TelegramBotClient client;
        private readonly IGetInlineKeyboardMarkup markup;
        private readonly ApplicationDbContext context;
        public GetToBackCommand(Bot bot, IGetInlineKeyboardMarkup markup,ApplicationDbContext context)
        {
            client = bot.GetBot();
            this.markup = markup;
            this.context = context;
        }
        public override string Name => "toBack";
        public override async Task ExecuteAsync(Update update)
        {
            var message = update.Message ?? update.CallbackQuery.Message;
            var toMainMarkup = markup.GetMain();

            var reminders = context.Reminders.Where(x => x.IsFinished == false).Where(x => x.ChatId == message.Chat.Id);
            if (reminders != null)
            {
                foreach (var reminder in reminders)
                {
                    context.Reminders.Remove(reminder);
                }
            }

            var user = context.Users.SingleOrDefault(x => x.ChatId == message.Chat.Id.ToString());

            if (user.IsDifferenceSet == false)
            {
                user.IsDifferenceSet = true;
            }

            if (user.IsDeleteReminder)
            {
                user.IsDeleteReminder = false;
            }

            await context.SaveChangesAsync();
            await client.EditMessageTextAsync(message.Chat.Id, message.MessageId, "Вот все, что я умею", replyMarkup: toMainMarkup);
        }
    }
}
