using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EmployeeHolidayTrackingSystem.Data.Models;

using static EmployeeHolidayTrackingSystem.Data.DataConstants.HolidayRequestStatusEnum;

namespace EmployeeHolidayTrackingSystem.Data
{
    public class EmployeeHolidayDbContext : IdentityDbContext<User>
    {
        private User employeeUser = null!;
        private User supervisorUser = null!;
        private Employee employee = null!;
        private Supervisor supervisor = null!;
        private IEnumerable<HolidayRequestStatus> holidayRequestStatuses = new List<HolidayRequestStatus>();

        public EmployeeHolidayDbContext(DbContextOptions<EmployeeHolidayDbContext> options)
            : base(options)
        {
            this.Database.Migrate();
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

            GenerateSeedData();

            builder
                .Entity<User>()
                .HasData(this.employeeUser, this.supervisorUser);

            builder
                .Entity<Supervisor>()
                .HasData(this.supervisor);

            builder
                .Entity<Employee>()
                .HasData(this.employee);

            builder
                .Entity<HolidayRequestStatus>()
                .HasData(this.holidayRequestStatuses);

            base.OnModelCreating(builder);
        }

        private void GenerateSeedData()
        {
            GenerateUsers();
            GenerateSupervisor();
            GenerateEmployee();
            GenerateHolidayRequestStatuses();
        }

        private void GenerateUsers()
        {
            var hasher = new PasswordHasher<User>();

            this.employeeUser = new User()
            {
                Id = "cbd615eb-e41f-48a7-9611-51d126377966",
                UserName = "employee@mail.com",
                NormalizedUserName = "EMPLOYEE@MAIL.COM",
                Email = "employee@mail.com",
                NormalizedEmail = "EMPLOYEE@MAIL.COM",
                FirstName = "Employee",
                LastName = "Andersen"
            };

            this.employeeUser.PasswordHash = hasher.HashPassword(this.employeeUser, "employee123#");

            this.supervisorUser = new User()
            {
                Id = "00ac9dd5-0bb7-46e2-b8d8-0cf24d426689",
                UserName = "supervisor@mail.com",
                NormalizedUserName = "SUPERVISOR@MAIL.COM",
                Email = "supervisor@mail.com",
                NormalizedEmail = "SUPERVISOR@MAIL.COM",
                FirstName = "Supervisor",
                LastName = "Jefferson"
            };

            this.supervisorUser.PasswordHash = hasher.HashPassword(this.supervisorUser, "supervisor123#");
        }

        private void GenerateSupervisor()
        {
            this.supervisor = new Supervisor()
            {
                Id = Guid.Parse("4d7c6287-5d0d-4c5a-a8ef-a01f7be06fa2"),
                UserId = this.supervisorUser.Id
            };
        }

        private void GenerateEmployee()
        {
            this.employee = new Employee()
            {
                Id = Guid.Parse("b80b70e0-683d-48bb-a10f-39892ee16f9c"),
                UserId = this.employeeUser.Id,
                SupervisorId = this.supervisor.Id
            };
        }

        private void GenerateHolidayRequestStatuses()
        {
            this.holidayRequestStatuses = new List<HolidayRequestStatus>()
            {
                new HolidayRequestStatus()
                {
                    Id = (int)Pending,
                    Title = Pending.ToString()
                },
                new HolidayRequestStatus()
                {
                    Id = (int)Approved,
                    Title = Approved.ToString()
                },
                new HolidayRequestStatus()
                {
                    Id = (int)Disapproved,
                    Title = Disapproved.ToString()
                }
            };
        }
    }
}