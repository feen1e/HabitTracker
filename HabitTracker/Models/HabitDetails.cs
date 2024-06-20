namespace HabitTracker.Models;

public class HabitDetails
{
    public Habit Habit { get; set; }
    //public List<HabitRecord> HabitRecords { get; set; } 
    public List<CalendarWeek> CalendarWeeks { get; set; }
}

public class CalendarDay
{
    public int DayNumber { get; set; }
    public string CssClass { get; set; }
}

public class CalendarWeek
{
    public List<CalendarDay> Days { get; set; }
}