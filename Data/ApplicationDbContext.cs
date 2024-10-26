using Microsoft.EntityFrameworkCore;
using minVagtPlan.Models.Entities;

namespace minVagtPlan.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Shift> Shifts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the many-to-many relationship between Shift and Employee
            modelBuilder.Entity<ShiftEmployee>()
                .HasKey(se => new { se.ShiftId, se.EmployeeId });

            modelBuilder.Entity<ShiftEmployee>()
                .HasOne(se => se.Shift)
                .WithMany(s => s.ShiftEmployees)
                .HasForeignKey(se => se.ShiftId);

            modelBuilder.Entity<ShiftEmployee>()
                .HasOne(se => se.Employee)
                .WithMany(e => e.ShiftEmployees)
                .HasForeignKey(se => se.EmployeeId);
        }
    }
}
