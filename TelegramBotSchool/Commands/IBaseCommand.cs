using Telegram.Bot.Types;

namespace TelegramBotSchool.Commands
{
    public interface IBaseCommand
    {
        public string Name { get; }
        public Task ExecuteAsync(Update update);
    }
}
