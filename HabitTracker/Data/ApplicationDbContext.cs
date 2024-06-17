using HabitTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Habit> Habits { get; set; }

        public DbSet<HabitRecord> HabitRecords { get; set; }
    }
}
