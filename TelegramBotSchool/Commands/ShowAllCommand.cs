using System.Text;
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
            var toBackMarkup = markup.GetToBack();
            var message = update.CallbackQuery.Message;

            if (context.Reminders.Any(x => x.ChatId == message.Chat.Id))
            {
                var reminders = context.Reminders.Where(x => x.ChatId == message.Chat.Id).OrderBy(x => x.ReminderTime);

                StringBuilder sb = new StringBuilder();
                int counter = 1;

                foreach (var reminder in reminders)
                {
                    sb.Append(counter + ") " + reminder.TextOfReminder + "\n" + reminder.ReminderTime + "\n");
                    counter++;
                }

                await client.EditMessageTextAsync(message.Chat.Id, message.MessageId, "Вот все, о чем мне надо напомнить \n" + $"{sb}",
                    replyMarkup: toBackMarkup);
            }
            else
            {
                await client.EditMessageTextAsync(message.Chat.Id, message.MessageId, "У вас нет напоминаний", replyMarkup: toBackMarkup);
            }
        }
    }
}
