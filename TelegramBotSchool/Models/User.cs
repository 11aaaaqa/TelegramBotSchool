using System.ComponentModel.DataAnnotations;

namespace TelegramBotSchool.Models
{
    public class User
    {
        [Key]
        public string ChatId { get; set; }
        public sbyte Difference { get; set; }
        public bool IsDifferenceSet { get; set; }
        public bool IsDeleteReminder { get; set; }
    }
}
