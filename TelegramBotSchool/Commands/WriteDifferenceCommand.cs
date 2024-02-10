using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotSchool.Database;
using TelegramBotSchool.Services;
using User = TelegramBotSchool.Models.User;

namespace TelegramBotSchool.Commands
{
    public class WriteDifferenceCommand : BaseCommand
    {
        private readonly TelegramBotClient client;
        private readonly ApplicationDbContext context;
        private readonly IGetInlineKeyboardMarkup markup;
        public WriteDifferenceCommand(Bot bot, ApplicationDbContext context, IGetInlineKeyboardMarkup markup)
        {
            this.context = context;
            client = bot.GetBot();
            this.markup = markup;
        }
        public override string Name => "WriteDifference";
        public override async Task ExecuteAsync(Update update)
        {
            var main = markup.GetMain();
            var message = update.Message;
            sbyte difference;
            try
            {
                difference = Convert.ToSByte(update.Message.Text);
            }
            catch (Exception e)
            {
                await client.SendTextMessageAsync(message.Chat.Id, $"'{update.Message.Text}' не является числом!");
                return;
            }

            await context.Users.AddAsync(new User { ChatId = message.Chat.Id.ToString(), Difference = difference, IsDifferenceSet = true, IsDeleteReminder = false});
            await context.SaveChangesAsync();

            await client.SendTextMessageAsync(message.Chat.Id, "Разница во времени успешно установлена",
                replyMarkup: main);
        }
    }
}
