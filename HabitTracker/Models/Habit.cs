namespace HabitTracker.Models
{
    public class Habit
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int Amount { get; set; }
        public string Unit { get; set; }
        public bool IsActive { get; set; }

        public ICollection<HabitRecord> HabitRecords { get; set; } = new List<HabitRecord>();
    }
}
