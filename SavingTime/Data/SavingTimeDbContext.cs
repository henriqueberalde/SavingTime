using Microsoft.EntityFrameworkCore;
using SavingTime.Entities;

namespace SavingTime.Data
{
    public class SavingTimeDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=saving_time;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False");
        }

        public DbSet<TimeRecord> TimeRecords { get; set; }
    }
}
