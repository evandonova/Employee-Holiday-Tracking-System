using Microsoft.EntityFrameworkCore;
using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;
using EmployeeHolidayTrackingSystem.Services.Users;
using EmployeeHolidayTrackingSystem.Services.Requests;
using EmployeeHolidayTrackingSystem.Services.Employees.Models;

using static EmployeeHolidayTrackingSystem.Data.DataConstants.Employee;

namespace EmployeeHolidayTrackingSystem.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeHolidayDbContext data;
        private readonly IUserService users;
        private readonly IRequestService requests;

        public EmployeeService(EmployeeHolidayDbContext data,
            IUserService users, IRequestService requests)
        {
            this.data = data;
            this.users = users;
            this.requests = requests;
        }

        public Guid GetEmployeeIdByUserId(string userId)
            => this.data.Employees.FirstOrDefault(e => e.UserId == userId)!.Id;

        public bool EmployeeExists(Guid id)
            => this.data.Employees.Any(e => e.Id == id);

        public string? GetEmployeeEmail(Guid id)
            => this.data.Employees
               .Where(e => e.Id == id)
               .Select(e => e.User.Email)
               .FirstOrDefault();

        public EmployeeDetailsServiceModel GetEmployeeDetails(Guid id)
            => this.data.Employees
                .Where(e => e.Id == id)
                .Select(e => new EmployeeDetailsServiceModel()
                {
                    Id = e.Id,
                    FirstName = e.User.FirstName,
                    LastName = e.User.LastName,
                    Email = e.User.Email,
                    HolidayDaysRemaining = e.HolidayDaysRemaining
                })
                .FirstOrDefault()!;

        public List<EmployeeServiceModel> GetSupervisorEmployees(Guid? supervisorId)
            => this.data.Employees
                .Where(e => e.SupervisorId == supervisorId)
                .Select(e => new EmployeeServiceModel()
                {
                    Id = e.Id,
                    FullName = $"{e.User.FirstName} {e.User.LastName}",
                })
                .ToList();

        public string GetEmployeeFullName(Guid id)
        {
            var employee = this.data.Employees.Include(e => e.User).FirstOrDefault(e => e.Id == id);
            return $"{employee?.User.FirstName} {employee?.User.LastName}" ?? string.Empty;
        }

        public Guid GetEmployeeSupervisorId(Guid employeeId)
            => this.data.Employees.FirstOrDefault(e => e.Id == employeeId)!.SupervisorId;

        public int? GetEmployeeHolidayDaysRemaining(Guid id)
            => this.data.Employees.Find(id)?.HolidayDaysRemaining;

        public bool CheckIfEmployeeHasEnoughHolidayDays(Guid? id, int days)
        {
            var employee = this.data.Employees.Find(id);
            return employee?.HolidayDaysRemaining > days;
        }

        public void CreateEmployee(string firstName, string lastName,
            string email, string password, Guid supervisorId, string employeeRoleName)
        {
            var newUserId = this.users.CreateUser(firstName, lastName, email, password);

            this.users.AddUserToRole(newUserId, employeeRoleName);

            var newEmployee = new Employee()
            {
                HolidayDaysRemaining = InitialHolidayDaysCount,
                UserId = newUserId,
                SupervisorId = supervisorId
            };

            this.data.Employees.Add(newEmployee);
            this.data.SaveChanges();
        }

        public void SubtractEmployeeHolidayDays(Guid id, int days)
        {
            var employee = this.data.Employees.Find(id);

            if (employee is null)
            {
                return;
            }

            employee.HolidayDaysRemaining -= days;
            this.data.SaveChanges();
        }

        public void EditEmployee(Guid id, string firstName, string lastName,
            string email, string? newPassword)
        {
            var employee = this.data.Employees
                .Include(e => e.User)
                .FirstOrDefault(e => e.Id == id);

            if (employee is null)
            {
                return;
            }

            employee.User.FirstName = firstName;
            employee.User.LastName = lastName;

            this.users.UpdateEmail(employee.UserId, email);

            if (newPassword is not null)
            {
                this.users.UpdatePassword(employee.UserId, newPassword);
            }

            this.data.SaveChanges();
        }

        public void DeleteEmployee(Guid id)
        {
            var employee = this.data.Employees.Find(id);

            if (employee is null)
            {
                return;
            }

            this.requests.DeleteEmployeeRequests(employee.Id);

            this.data.Employees.Remove(employee);
            this.data.SaveChanges();

            this.users.DeleteUser(employee.UserId);
        }

        public void DeleteSupervisorEmployees(Guid supervisorId)
        {
            var supervisor = this.data.Supervisors.Include(s => s.Employees)
                .FirstOrDefault(s => s.Id == supervisorId);

            if (supervisor is null)
            {
                return;
            }

            foreach (var employee in supervisor.Employees)
            {
                this.requests.DeleteEmployeeRequests(employee.Id);
            }

            var employeeUserIds = supervisor.Employees.Select(e => e.UserId).ToList();

            this.data.Employees.RemoveRange(supervisor.Employees);
            this.data.SaveChanges();

            foreach (var userId in employeeUserIds)
            {
                this.users.DeleteUser(userId);
            }
        }

        public EmployeeServiceModel? GetEmployeeProfileData(string? userId)
            => this.data.Employees
                .Where(e => e.UserId == userId)
                .Select(e => new EmployeeServiceModel()
                {
                    Id = e.Id,
                    FullName = $"{e.User.FirstName} {e.User.LastName}",
                    SupervisorName = $"{e.Supervisor.User.FirstName} {e.Supervisor.User.LastName}",
                    HolidayDaysRemaining = e.HolidayDaysRemaining
                })
                .FirstOrDefault();
    }
}
