using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using TelegramBotSchool.Database;

namespace TelegramBotSchool.Commands
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly List<BaseCommand> commands;
        private BaseCommand lastCommand;
        private ApplicationDbContext context;

        public CommandExecutor(IServiceProvider provider, ApplicationDbContext context)
        {
            commands = provider.GetServices<BaseCommand>().ToList();
            this.context = context;
        }
        public async Task Execute(Update update)
        {
            var message = update.Message;
            if (message != null)
            {
                if (message!.Text != "/start" && await context.Users.SingleOrDefaultAsync(x => x.ChatId == message.Chat.Id.ToString()) == null)
                {
                    await ExecuteCommand("WriteDifference", update);
                }

                if (message.Text == "/start")
                {
                    await ExecuteCommand("Start", update);
                }

                if (context.Users.SingleOrDefault(x => x.ChatId == message.Chat.Id.ToString()) != null && 
                    context.Users.SingleOrDefault(x => x.ChatId == message.Chat.Id.ToString())!.IsDifferenceSet == false)
                {

                }
            }

            switch (update.CallbackQuery?.Data)
            {
                case "showAll":
                    await ExecuteCommand("", update);
                    break;
                case "add":
                    await ExecuteCommand("", update);
                    break;
                case "delete":
                    await ExecuteCommand("", update);
                    break;
                case "changeDifference":
                    await ExecuteCommand("changeDifference", update);
                    break;
                case "toBack":
                    await ExecuteCommand("", update);
                    break;
            }
        }
        private async Task ExecuteCommand(string commandName, Update update)
        {
            lastCommand = commands.First(x => x.Name == commandName);
            await lastCommand.ExecuteAsync(update);
        }
    }
}
