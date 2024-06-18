namespace HabitTracker.Models;

public class HabitDetails
{
    public Habit Habit { get; set; }
    public List<HabitRecord> HabitRecords { get; set; } 
}