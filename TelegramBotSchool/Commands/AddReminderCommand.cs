using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotSchool.Database;
using TelegramBotSchool.Models;
using TelegramBotSchool.Services;

namespace TelegramBotSchool.Commands
{
    public class AddReminderCommand : BaseCommand
    {
        private readonly TelegramBotClient client;
        private readonly IGetInlineKeyboardMarkup markup;
        private readonly ApplicationDbContext context;

        public AddReminderCommand(Bot bot, IGetInlineKeyboardMarkup markup, ApplicationDbContext context)
        {
            client = bot.GetBot();
            this.markup = markup;
            this.context = context;
        }
        public override string Name => "add";
        public override async Task ExecuteAsync(Update update)
        {
            var message = update.CallbackQuery.Message;
            var toBackMarkup = markup.GetToBack();
            long maxReminderId;

            if (context.Reminders.FirstOrDefault(x => x.ChatId == message.MessageId) == null) 
                maxReminderId = 0;
            else
            {
                maxReminderId = context.Reminders.Where(x => x.ChatId == message.Chat.Id)
                    .Max(x => x.ReminderId);
            }

            await context.Reminders.AddAsync(new Reminder
            {
                ChatId = message.Chat.Id, IsFinished = false, ReminderId = maxReminderId + 1,Id = Guid.NewGuid(),TextOfReminder = "null"
            });

            await context.SaveChangesAsync();

            await client.EditMessageTextAsync(message.Chat.Id,
                message.MessageId, "Введите текст напоминания", replyMarkup: toBackMarkup);
        }
    }
}
