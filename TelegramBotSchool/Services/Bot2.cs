using Telegram.Bot;

namespace TelegramBotSchool.Services
{
    public class Bot
    {
        private TelegramBotClient client;

        public TelegramBotClient GetBot()
        {
            if (client != null) return client;

            client = new TelegramBotClient("6718004564:AAFeLov8L4ZKY8y7cRhODMK4NARuc3al-iU");

            return client;
        }
    }
}
