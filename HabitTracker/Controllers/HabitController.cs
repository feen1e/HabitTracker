﻿using HabitTracker.Data;
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
            foreach (var habit in habits)
            {
                bool hasRecordToday = habit.HabitRecords.Any(hr => hr.Date.Date == DateTime.Today && hr.IsCompleted);
                if (!hasRecordToday)
                {
                    habitsRemaining++;
                }
                foreach (var a in habit.HabitRecords.Select(hr => hr))
                {
                    Console.WriteLine($"{a.Date} === {DateTime.Today} wykonany: {(a.IsCompleted ? "tak" : "nie")}");
                };
            }
            Console.WriteLine(habitsRemaining + " pozostalo");
            var habitHome = new HabitHomeView(habits, habitsRemaining);
            
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
    }
}