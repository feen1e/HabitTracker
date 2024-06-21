namespace HabitTracker.Models;

public class HabitHomeView(List<Habit> habits, int habitsRemaining, List<HabitRecord> recordsToday)
{
    public List<Habit> Habits { get; set; } = habits;
    public List<HabitRecord> RecordsToday { get; set; } = recordsToday;
    public int HabitsRemainingToday { get; set; } = habitsRemaining;
}