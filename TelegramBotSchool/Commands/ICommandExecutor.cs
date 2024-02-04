using Telegram.Bot.Types;

namespace TelegramBotSchool.Commands
{
    public interface ICommandExecutor
    {
        Task Execute(Update update);
    }
}
