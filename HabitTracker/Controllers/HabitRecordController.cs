using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Controllers
{
    public class HabitRecordController : Controller
    {
        private ApplicationDbContext db;
        public HabitRecordController(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MS SQL"));
            db = new ApplicationDbContext(optionsBuilder.Options);
        }


        public ActionResult Index()
        {
            var habitRecords = db.HabitRecords.Include("Habit").ToList();
            return View(habitRecords.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.HabitId = new SelectList(db.Habits, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,HabitId,Date,IsCompleted")] HabitRecord habitRecord)
        {
            if (ModelState.IsValid)
            {
                db.HabitRecords.Add(habitRecord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HabitId = new SelectList(db.Habits, "Id", "Name", habitRecord.HabitId);
            return View(habitRecord);
        }
    }
}
