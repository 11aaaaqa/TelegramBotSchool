using System.ComponentModel.DataAnnotations;

namespace TelegramBotSchool.Models
{
    public class Reminder
    {
        [Key]
        public Guid Id { get; set; }
        public long ChatId { get; set; }
        public long ReminderId { get; set; }
        public string TextOfReminder { get; set; }
        public DateTime ReminderTime { get; set; }
        public bool IsFinished { get; set; }
    }
}
