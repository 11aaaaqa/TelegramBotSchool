using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotSchool.Database;
using TelegramBotSchool.Services;

namespace TelegramBotSchool.Commands
{
    public class ChangeDifferenceExecutor : BaseCommand
    {
        private readonly ApplicationDbContext context;
        private readonly TelegramBotClient client;
        private readonly IGetInlineKeyboardMarkup markup;
        public ChangeDifferenceExecutor(ApplicationDbContext context, Bot bot, IGetInlineKeyboardMarkup markup)
        {
            this.context = context;
            client = bot.GetBot();
            this.markup = markup;
        }
        public override string Name => "change";
        public override async Task ExecuteAsync(Update update)
        {
            var message = update.Message;
            var toBackMarkup = markup.GetToBack();
            var mainMarkup = markup.GetMain();
            var user = await context.Users.SingleAsync(x => x.ChatId == message.Chat.Id.ToString());

            sbyte difference;
            try
            {
                difference = Convert.ToSByte(message.Text);
            }
            catch (Exception e)
            {
                DateTime utc = DateTime.UtcNow;
                await client.SendTextMessageAsync(message.Chat.Id, $"'{update.Message.Text}' не является числом!\n\n\n" + 
                                                                   "Пожалуйста, введите вашу разницу с всемирным координированным временем (Например: 1, -1, 0)\n\n" +
                                                                   $"Текущее всемирное координированное время: {utc}\n\nВаша текущая разница во времени: {user.Difference}", replyMarkup: toBackMarkup);
                return;
            }

            user.Difference = difference;
            user.IsDifferenceSet = true;

            await context.SaveChangesAsync();

            await client.SendTextMessageAsync(message.Chat.Id, "Разница во времени успешно установлена",
                replyMarkup: mainMarkup);
        }
    }
}
