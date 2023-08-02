using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;
using EmployeeHolidayTrackingSystem.Services.Users;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Services.Supervisors.Models;
using Microsoft.EntityFrameworkCore;

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

        public string? GetSupervisorFullName(Guid? supervisorId)
            => this.data.Supervisors
                .Where(s => s.Id == supervisorId)
                .Select(s => $"{s.User.FirstName} {s.User.LastName}")
                .FirstOrDefault();

        public Guid GetSupervisorIdByUserId(string userId)
            => this.data.Supervisors.FirstOrDefault(s => s.UserId == userId)!.Id;

        public List<SupervisorServiceModel> GetAll()
            => this.data.Supervisors
            .Select(s => new SupervisorServiceModel()
            {
                Id = s.Id,
                FullName = $"{s.User.FirstName} {s.User.LastName}"
            })
            .ToList();

        public SupervisorDetailsServiceModel? GetDetails(Guid id)
            => this.data.Supervisors
            .Where(s => s.Id == id)
            .Select(s => new SupervisorDetailsServiceModel()
            {
                Id = s.Id,
                FirstName = s.User.FirstName,
                LastName = s.User.LastName,
                Email = s.User.Email
            })
            .FirstOrDefault();

        public bool SupervisorExists(Guid id)
            => this.data.Supervisors.Any(s => s.Id == id);

        public string? GetSupervisorEmail(Guid id)
           => this.data.Supervisors
              .Where(s => s.Id == id)
              .Select(s => s.User.Email)
              .FirstOrDefault();

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
            var supervisor = this.data.Employees
                .Include(s => s.User)
                .FirstOrDefault(s => s.Id == id);

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
