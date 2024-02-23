using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotSchool.Database;
using TelegramBotSchool.Services;

namespace TelegramBotSchool.Commands
{
    public class StartCommand : IBaseCommand
    {   
        private readonly TelegramBotClient client;
        private readonly ApplicationDbContext context;
        private readonly IGetInlineKeyboardMarkup markup;
        public StartCommand(Bot bot, ApplicationDbContext context, IGetInlineKeyboardMarkup markup)
        {
            this.context = context;
            client = bot.GetBot();
            this.markup = markup;
        }
        public string Name => "Start";
        public async Task ExecuteAsync(Update update)
        {
            var main = markup.GetMain();

            var message = update.Message;

            if (await context.Users.SingleOrDefaultAsync(x => x.ChatId == message.Chat.Id.ToString()) == null)
            {
                DateTime utc = DateTime.UtcNow;

                await client.SendTextMessageAsync(message.Chat.Id,
                    $"Пожалуйста, введите вашу разницу с всемирным координированным временем (Например: 1, -1, 0)\n\nТекущее всемирное координированное время: {utc}");
            }
            else
            {
                if (context.Reminders.Where(x => x.IsFinished == false).FirstOrDefault(x => x.ChatId == message.Chat.Id) != null)
                {
                    var reminders = context.Reminders.Where(x => x.IsFinished == false).Where(x => x.ChatId == message.Chat.Id);
                    foreach (var reminder in reminders)
                    {
                        context.Reminders.Remove(reminder);
                    }
                }

                await context.SaveChangesAsync();

                await client.SendTextMessageAsync(message.Chat.Id, "Привет, я могу напомнить тебе о чем угодно!", replyMarkup: main);
            }
        }
    }
}
