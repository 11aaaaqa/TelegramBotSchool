namespace TelegramBotSchool.Models
{
    public class Reminder
    {
        public long ChatId { get; set; }
        public long ReminderId { get; set; }
        public string TextOfReminder { get; set; }
        public DateTime ReminderTime { get; set; }
    }
}
