using Microsoft.EntityFrameworkCore;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotSchool.Database;
using TelegramBotSchool.Services;

namespace TelegramBotSchool.Commands
{
    public class DeleteReminderCommand : IBaseCommand
    {
        private readonly TelegramBotClient client;
        private readonly IGetInlineKeyboardMarkup markup;
        private readonly ApplicationDbContext context;
        public DeleteReminderCommand(Bot bot, IGetInlineKeyboardMarkup markup, ApplicationDbContext context)
        {
            client = bot.GetBot();
            this.markup = markup;
            this.context = context;
        }
        public string Name => "delete";
        public async Task ExecuteAsync(Update update)
        {
            var toBackMarkup = markup.GetToBack();
            var message = update.CallbackQuery.Message;
            var reminders = await context.Reminders.Where(x => x.ChatId == message.Chat.Id).OrderBy(x => x.ReminderId).ToListAsync();

            if (reminders.Count == 0)
            {
                await client.EditMessageTextAsync(message.Chat.Id, message.MessageId, "У вас нет напоминаний",
                    replyMarkup:toBackMarkup);
                return;
            }

            StringBuilder sb = new StringBuilder();
            
            foreach (var reminder in reminders)
            {
                sb.Append(reminder.ReminderId + ") " + reminder.TextOfReminder + "\n" + reminder.ReminderTime + "\n");
            }

            var user = await context.Users.SingleOrDefaultAsync(x => x.ChatId == message.Chat.Id.ToString());
            user.IsDeleteReminder = true;

            await context.SaveChangesAsync();
            await client.EditMessageTextAsync(message.Chat.Id, message.MessageId, "Введите номер напоминания для удаления \n" + $"{sb}",
                replyMarkup:toBackMarkup);
        }
    }
}
