using Microsoft.EntityFrameworkCore;
using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;
using EmployeeHolidayTrackingSystem.Services.Users;
using EmployeeHolidayTrackingSystem.Services.Employees;

namespace EmployeeHolidayTrackingSystem.Services.Supervisors
{
    public class SupervisorService : ISupervisorService
    {
        private readonly EmployeeHolidayDbContext data;
        private readonly IUserService users;
        private readonly IEmployeeService employees;

        public SupervisorService(EmployeeHolidayDbContext data,
            IUserService users, IEmployeeService employees)
        {
            this.data = data;
            this.users = users;
            this.employees = employees;
        }

        public Supervisor? GetSupervisorById(Guid id)
            => this.data.Supervisors
                    .Include(e => e.User)
                    .FirstOrDefault(e => e.Id == id);

        public Supervisor? GetSupervisorByUserId(string? userId)
            => this.data.Supervisors
                    .Include(s => s.User)
                    .Include(s => s.Employees)
                    .Include(s => s.HolidayRequests)
                    .FirstOrDefault(s => s.UserId == userId);

        public List<Supervisor> GetAll()
            => this.data.Supervisors.ToList();

        public void CreateSupervisor(string firstName, string lastName,
            string email, string password, string supervisorRoleName)
        {
            var newUserId = this.users.CreateUser(firstName, lastName, email, password);

            this.users.AddUserToRole(newUserId, supervisorRoleName);

            var newSupervisor = new Supervisor()
            {
                UserId = newUserId
            };

            this.data.Supervisors.Add(newSupervisor);
            this.data.SaveChanges();
        }

        public void EditSupervisor(Guid id, string firstName, string lastName,
            string email, string? newPassword)
        {
            var supervisor = this.data.Supervisors.Find(id);

            if (supervisor is null)
            {
                return;
            }

            supervisor.User.FirstName = firstName;
            supervisor.User.LastName = lastName;

            this.users.UpdateEmail(supervisor.UserId, email);

            if (newPassword is not null)
            {
                this.users.UpdatePassword(supervisor.UserId, newPassword);
            }

            this.data.SaveChanges();
        }

        public void DeleteSupervisor(Guid id)
        {
            var supervisor = this.data.Supervisors.Find(id);

            if (supervisor is null)
            {
                return;
            }

            this.employees.DeleteSupervisorEmployees(supervisor.Id);

            this.data.Supervisors.Remove(supervisor);
            this.data.SaveChanges();

            this.users.DeleteUser(supervisor.UserId);
        }
    }
}
