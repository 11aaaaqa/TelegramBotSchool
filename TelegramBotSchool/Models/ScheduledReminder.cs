using System.ComponentModel.DataAnnotations;

namespace TelegramBotSchool.Models
{
    public class ScheduledReminder
    {
        [Key]
        public Guid ReminderId { get; set; }
        public string JobId { get; set; }
    }
}
