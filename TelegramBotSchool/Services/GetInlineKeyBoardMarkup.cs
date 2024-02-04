using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotSchool.Services
{
    public class GetInlineKeyBoardMarkup : IGetInlineKeyboardMarkup
    {
        public InlineKeyboardMarkup GetMain()
        {
            InlineKeyboardMarkup main = new(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Все напоминания", callbackData: "showAll"),
                    InlineKeyboardButton.WithCallbackData(text: "Добавить", callbackData: "add")
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Удалить", callbackData: "delete"),
                    InlineKeyboardButton.WithCallbackData(text: "Изменить разницу во времени", callbackData: "changeDifference")
                }
            });
            return main;
        }

        public InlineKeyboardMarkup GetToBack()
        {
            InlineKeyboardMarkup toBack = new(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "На главную", callbackData: "toBack")
                }
            });
            return toBack;
        }
    }
}
