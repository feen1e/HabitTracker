using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;
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
            return View(db.Habits.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        public IActionResult Create([Microsoft.AspNetCore.Mvc.Bind("Id,Name,Description,IsActive")] Habit habit)
        {
            if (ModelState.IsValid)
            {
                db.Habits.Add(habit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(habit);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habit = await db.Habits
                .Include(h => h.HabitRecords)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (habit == null)
            {
                return NotFound();
            }

            var model = new HabitDetails
            {
                Habit = habit,
                HabitRecords = habit.HabitRecords.ToList()
            };

            return View(model);
        }


        [Microsoft.AspNetCore.Mvc.HttpPost]
        public ActionResult ToggleCompletion(int habitId, DateTime date)
        {
            var record = db.HabitRecords.FirstOrDefault(hr => hr.HabitId == habitId && hr.Date == date);
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

            db.SaveChanges();
            return RedirectToAction("Details", new { id = habitId });
        }

        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,IsActive")] Habit habit)
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
    }
}