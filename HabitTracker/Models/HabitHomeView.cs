namespace HabitTracker.Models;

public class HabitHomeView(List<Habit> habits, int habitsRemaining)
{
    public List<Habit> Habits { get; set; } = habits;
    public int HabitsRemainingToday { get; set; } = habitsRemaining;
}