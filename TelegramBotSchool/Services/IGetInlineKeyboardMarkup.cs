using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotSchool.Services
{
    public interface IGetInlineKeyboardMarkup
    {
        InlineKeyboardMarkup GetMain();
        InlineKeyboardMarkup GetToBack();
    }
}
