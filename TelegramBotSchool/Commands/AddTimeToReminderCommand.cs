using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotSchool.Database;
using TelegramBotSchool.Models;
using TelegramBotSchool.Services;

namespace TelegramBotSchool.Commands
{
    public class AddTimeToReminderCommand  : IBaseCommand
    {
        private readonly TelegramBotClient client;
        private readonly ApplicationDbContext context;
        private readonly IGetInlineKeyboardMarkup markup;

        public AddTimeToReminderCommand(Bot bot, ApplicationDbContext context, IGetInlineKeyboardMarkup markup)
        {
            client = bot.GetBot();
            this.context = context;
            this.markup = markup;
        }
        public string Name => "addTimeToReminder";
        public async Task ExecuteAsync(Update update)
        {
            var message = update.Message;
            var mainMarkup = markup.GetMain();
            var toBackMarkup = markup.GetToBack();
            string formats = "\nчасы:минуты\nчасы:минуты:секунды\nдень.месяц.год  часы:минуты\nдень.месяц.год  часы:минуты:секунды";

            DateTime reminderTime = new DateTime();
            try
            {
                reminderTime = Convert.ToDateTime(message.Text);
            }
            catch (Exception ex)
            {
                await client.SendTextMessageAsync(message.Chat.Id,
                    "Неверный формат\n\nВведите дату в формате: " + formats, replyMarkup: toBackMarkup);
                return;
            }

            var userDifference = context.Users.Single(x => x.ChatId == message.Chat.Id.ToString()).Difference;

            var nowUserTime = DateTime.UtcNow.AddHours(userDifference);

            if (reminderTime > nowUserTime)
            {
                var reminder = await context.Reminders.Where(x => x.IsFinished == false).SingleOrDefaultAsync(x => x.ChatId == message.Chat.Id);

                reminder.IsFinished = true;

                reminder.ReminderTime = reminderTime;

                var user = await context.Users.SingleOrDefaultAsync(x => x.ChatId == message.Chat.Id.ToString());
                var jobId = BackgroundJob.Schedule(() => MethodToRun(reminder, message.Chat.Id, reminder.TextOfReminder),
                    reminderTime - DateTime.UtcNow.AddHours(user.Difference));

                await context.ScheduledReminders.AddAsync(new ScheduledReminder{JobId = jobId, ReminderId = reminder.Id});

                await context.SaveChangesAsync();

                await client.SendTextMessageAsync(message.Chat.Id, "Напоминание успешно добавлено",
                    replyMarkup: mainMarkup);
            }
            else
            {
                await client.SendTextMessageAsync(message.Chat.Id, "Время должно быть в будущем", replyMarkup:toBackMarkup);
            }
        }

        public async Task MethodToRun(Reminder reminder, long chatId,string textOfReminder)
        {
            await client.SendTextMessageAsync(chatId, textOfReminder);

            var remndr = await context.Reminders.SingleOrDefaultAsync(x => x.Id == reminder.Id);

            var scheduledReminder = await context.ScheduledReminders.SingleOrDefaultAsync(x => x.ReminderId == reminder.Id);
            
            context.ScheduledReminders.Remove(scheduledReminder);
            context.Reminders.Remove(remndr);

            await context.SaveChangesAsync();
        }
    }
}
