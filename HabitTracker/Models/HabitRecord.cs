﻿namespace HabitTracker.Models
{
    public class HabitRecord
    {
        public int Id { get; set; }
        public int HabitId { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }

        public virtual Habit Habit { get; set; }
    }
}
