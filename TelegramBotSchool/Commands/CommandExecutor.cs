﻿using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using TelegramBotSchool.Database;

namespace TelegramBotSchool.Commands
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly List<IBaseCommand> commands;
        private ApplicationDbContext context;

        public CommandExecutor(IServiceProvider provider, ApplicationDbContext context)
        {
            commands = provider.GetServices<IBaseCommand>().ToList();
            this.context = context;
        }
        public async Task Execute(Update update)
        {
            var message = update.Message;

            switch (update.CallbackQuery?.Data)
            {
                case "showAll":
                    await ExecuteCommand("showAll", update);
                    break;
                case "add":
                    await ExecuteCommand("add", update);
                    break;
                case "delete":
                    await ExecuteCommand("delete", update);
                    break;
                case "changeDifference":
                    await ExecuteCommand("changeDifference", update);
                    break;
                case "toBack":
                    await ExecuteCommand("toBack", update);
                    break;
            }


            if (message != null)
            {
                var user = await context.Users.SingleOrDefaultAsync(x => x.ChatId == message.Chat.Id.ToString());
                if (user != null && user.IsDeleteReminder)
                {
                    await ExecuteCommand("deleteReminderExecutor", update);
                    return;
                }

                if (message!.Text != "/start" && user == null)
                {
                    await ExecuteCommand("WriteDifference", update);
                }

                if (context.Reminders.Where(x => x.ChatId == message.Chat.Id).Any(x => x.IsFinished == false))
                {
                    if (context.Reminders.Where(x => x.IsFinished == false).FirstOrDefault(x => x.ChatId == message.Chat.Id)?.TextOfReminder == "null")
                    {
                        await ExecuteCommand("addTextToReminder", update);
                        return;
                    }
                    {
                        await ExecuteCommand("addTimeToReminder", update);
                        return;
                    }
                }

                if (user != null && user.IsDifferenceSet == false)
                {
                    await ExecuteCommand("change", update);
                    return;
                }

                if (message.Text == "/start")
                {
                    await ExecuteCommand("Start", update);
                }
            }
        }
        private async Task ExecuteCommand(string commandName, Update update)
        {
            var lastCommand = commands.Single(x => x.Name == commandName);
            await lastCommand.ExecuteAsync(update);
        }
    }
}
