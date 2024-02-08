using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotSchool.Database;
using TelegramBotSchool.Services;

namespace TelegramBotSchool.Commands
{
    public class AddTextToReminderCommand :BaseCommand
    {
        private readonly TelegramBotClient client;
        private readonly ApplicationDbContext context;
        private readonly IGetInlineKeyboardMarkup markup;
        public AddTextToReminderCommand(Bot bot, ApplicationDbContext context, IGetInlineKeyboardMarkup markup)
        {
            client = bot.GetBot();
            this.context = context;
            this.markup = markup;
        }
        public override string Name => "addTextToReminder";
        public override async Task ExecuteAsync(Update update)
        {
            var message = update.Message;
            var toBackMarkup = markup.GetToBack();
            string formats = "\nчасы:минуты\nчасы:минуты:секунды\nдень.месяц.год  часы:минуты\nдень.месяц.год  часы:минуты:секунды";

            var reminder = 
                await context.Reminders.Where(x => x.ChatId == message.Chat.Id).SingleOrDefaultAsync(x => x.IsFinished == false);

            reminder.TextOfReminder = message.Text;
            
            await context.SaveChangesAsync();

            await client.SendTextMessageAsync(message.Chat.Id, "Введите время напоминания в формате: " + formats, replyMarkup: toBackMarkup);
        }
    }
}
