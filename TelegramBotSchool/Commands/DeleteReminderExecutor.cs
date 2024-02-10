using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotSchool.Database;
using TelegramBotSchool.Services;

namespace TelegramBotSchool.Commands
{
    public class DeleteReminderExecutor : BaseCommand
    {
        private readonly ApplicationDbContext context;
        private readonly TelegramBotClient client;
        private readonly IGetInlineKeyboardMarkup markup;

        public DeleteReminderExecutor(Bot bot, ApplicationDbContext context, IGetInlineKeyboardMarkup markup)
        {
            client = bot.GetBot();
            this.context = context;
            this.markup = markup;
        }

        public override string Name => "deleteReminderExecutor";

        public override async Task ExecuteAsync(Update update)
        {
            var message = update.Message;
            var toBackMarkup = markup.GetToBack();
            var mainMarkup = markup.GetMain();

            int deleteNumber;
            try
            {
                deleteNumber = Convert.ToInt32(message.Text);
            }
            catch (Exception e)
            {
                await client.SendTextMessageAsync(message.Chat.Id,
                    "Такого напоминания не существует. Введите номер существующего напоминания для удаления",
                    replyMarkup: toBackMarkup);
                return;
            }

            var reminder = await context.Reminders.Where(x => x.ChatId == message.Chat.Id)
                .SingleOrDefaultAsync(x => x.ReminderId == deleteNumber);

            if (reminder == null)
            {
                await client.SendTextMessageAsync(message.Chat.Id,
                    "Такого напоминания не существует. Введите номер существующего напоминания для удаления",
                    replyMarkup: toBackMarkup);
                return;
            }

            context.Reminders.Remove(reminder);

            var user = await context.Users.SingleOrDefaultAsync(x => x.ChatId == message.Chat.Id.ToString());
            user.IsDeleteReminder = false;

            #region Уменьшение каждого Id, который больше удаленного
            var remndrs = context.Reminders.Where(x => x.ChatId == message.Chat.Id)
                .Where(x => x.ReminderId > deleteNumber).ToList();


            if (remndrs.Count != 0)
            {
                foreach (var remndr in remndrs)
                {
                    remndr.ReminderId--;
                }
            }
            #endregion

            await context.SaveChangesAsync();

            await client.SendTextMessageAsync(message.Chat.Id, "Вы успешно удалили напоминание",
                replyMarkup: mainMarkup);
        }
    }
}
