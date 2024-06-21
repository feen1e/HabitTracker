using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace HabitTracker.Controllers
{
    public class HabitController : Controller
    {
        private readonly ApplicationDbContext db;

        public HabitController(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MS SQL"));
            db = new ApplicationDbContext(optionsBuilder.Options);
        }

        public IActionResult Index()
        {
            var habits = db.Habits.Include(habit => habit.HabitRecords).ToList();
            int habitsRemaining = 0;
            List<HabitRecord> habitRecordsToday = new();
            foreach (var habit in habits)
            {
                bool hasRecordToday = habit.HabitRecords.Any(hr => hr.Date.Date == DateTime.Today && hr.IsCompleted);
                if (!hasRecordToday)
                {
                    habitsRemaining++;
                }
                else
                {
                    habitRecordsToday.Add(habit.HabitRecords.First(hr => hr.Date == DateTime.Today && hr.IsCompleted));
                }
                foreach (var a in habit.HabitRecords.Select(hr => hr))
                {
                    Console.WriteLine($"{a.Date} === {DateTime.Today} wykonany: {(a.IsCompleted ? "tak" : "nie")}");
                };
            }
            Console.WriteLine(habitsRemaining + " pozostalo");
            var habitHome = new HabitHomeView(habits, habitsRemaining, habitRecordsToday);
            
            return View(habitHome);
        }
        
        public IActionResult List()
        {
            return View(db.Habits.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Unit,Amount,IsActive")] Habit habit)
        {
            if (ModelState.IsValid)
            {
                db.Habits.Add(habit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(habit);
        }
        
        [HttpPost]
        public async Task<IActionResult> ToggleCompletion(int habitId, DateTime date)
        {
            date = date.Date;

            var record = await db.HabitRecords
                .FirstOrDefaultAsync(hr => hr.HabitId == habitId && hr.Date == date);

            if (record == null)
            {
                record = new HabitRecord { HabitId = habitId, Date = date, IsCompleted = true };
                db.HabitRecords.Add(record);
            }
            else
            {
                record.IsCompleted = !record.IsCompleted;
                db.Entry(record).State = EntityState.Modified;
            }

            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = habitId });
        }
        
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Amount,Unit,IsActive")] Habit habit)
        {
            if (id != habit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(habit);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HabitExists(habit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(habit);
        }

        private bool HabitExists(int id)
        {
            return db.Habits.Any(e => e.Id == id);
        }
        
        public async Task<IActionResult> Details(int id)
        {
            var habit = await db.Habits
                .Include(h => h.HabitRecords)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (habit == null)
            {
                return NotFound();
            }

            var calendarWeeks = GenerateCalendarWeeks(habit.HabitRecords.ToList());

            var viewModel = new HabitDetails()
            {
                Habit = habit,
                CalendarWeeks = calendarWeeks
            };

            return View(viewModel);
        }

        private List<CalendarWeek> GenerateCalendarWeeks(List<HabitRecord> records)
        {
            List<CalendarWeek> calendarWeeks = new List<CalendarWeek>();

            DateTime currentDate = DateTime.Today;
            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            int daysUntilMonday = ((int)firstDayOfMonth.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            DateTime firstMondayOfMonth = firstDayOfMonth.AddDays(-daysUntilMonday);

            for (int weekIndex = 0; weekIndex < 4; weekIndex++)
            {
                CalendarWeek week = new CalendarWeek();
                week.Days = new List<CalendarDay>();
                
                for (int dayIndex = 0; dayIndex < 7; dayIndex++)
                {
                    DateTime currentDay = firstMondayOfMonth.AddDays(weekIndex * 7 + dayIndex);
                    
                    bool isPreviousMonth = currentDay.Month != currentDate.Month;
                    
                    bool hasRecord = records.Any(r => r.Date.Date == currentDay.Date);

                    string cssClass = hasRecord ? "marked" : "not-marked";

                    if (isPreviousMonth)
                    {
                        cssClass += " previous-month";
                    }
                    
                    week.Days.Add(new CalendarDay { DayNumber = currentDay.Day, CssClass = cssClass });
                }

                calendarWeeks.Add(week);
            }

            return calendarWeeks;
        }
    }
}