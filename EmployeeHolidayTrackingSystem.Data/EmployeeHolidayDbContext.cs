using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EmployeeHolidayTrackingSystem.Data.Models;

namespace EmployeeHolidayTrackingSystem.Data
{
    public class EmployeeHolidayDbContext : IdentityDbContext<User>
    {
        public EmployeeHolidayDbContext(DbContextOptions<EmployeeHolidayDbContext> options)
            : base(options)
        {
        }

        public DbSet<Supervisor> Supervisors { get; init; } = null!;

        public DbSet<Employee> Employees { get; init; } = null!;

        public DbSet<HolidayRequestStatus> HolidayRequestStatuses { get; init; } = null!;

        public DbSet<HolidayRequest> HolidayRequests { get; init; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Employee>()
                .HasOne(e => e.Supervisor)
                .WithMany(s => s.Employees)
                .HasForeignKey(e => e.SupervisorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Supervisor>()
                .HasMany(s => s.Employees)
                .WithOne(e => e.Supervisor);

            builder
                .Entity<HolidayRequest>()
                .HasOne(h => h.Status)
                .WithMany(h => h.HolidayRequests)
                .HasForeignKey(h => h.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<HolidayRequest>()
                .HasOne(h => h.Employee)
                .WithMany(e => e.HolidayRequests)
                .HasForeignKey(h => h.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<HolidayRequest>()
                .HasOne(h => h.Supervisor)
                .WithMany(s => s.PendingHolidayRequests)
                .HasForeignKey(h => h.SupervisorId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}