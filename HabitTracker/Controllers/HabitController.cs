using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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


        public ActionResult Index()
        {
            return View(db.Habits.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Name,Description,IsActive")] Habit habit)
        {
            if (ModelState.IsValid)
            {
                db.Habits.Add(habit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(habit);
        }
    }
}
