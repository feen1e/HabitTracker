using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    public sealed class HabitRecord
    {
        public int Id { get; set; }
        public int HabitId { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }

        public Habit Habit { get; set; }
    }
}
