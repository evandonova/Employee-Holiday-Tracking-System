using Microsoft.EntityFrameworkCore;
using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;
using EmployeeHolidayTrackingSystem.Services.Users;
using EmployeeHolidayTrackingSystem.Services.Requests;

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

        public Employee? GetEmployeeByUserId(string? userId)
            => this.data.Employees
                    .Include(e => e.User)
                    .Include(e => e.Supervisor.User)
                    .Include(e => e.HolidayRequests)
                    .FirstOrDefault(e => e.UserId == userId);

        public Employee? GetEmployeeById(Guid id)
            => this.data.Employees
                    .Include(e => e.User)
                    .FirstOrDefault(e => e.Id == id);

        public string GetEmployeeFullName(Guid id)
        {
            var employee = this.data.Employees.Include(e => e.User).FirstOrDefault(e => e.Id == id);
            return $"{employee?.User.FirstName} {employee?.User.LastName}" ?? string.Empty;
        }

        public int? GetEmployeeHolidayDaysRemaining(Guid id)
            => this.data.Employees.Find(id)?.HolidayDaysRemaining;

        public bool CheckIfEmployeeHasEnoughHolidayDays(Guid id, int days)
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
            var employee = this.data.Employees.Find(id);
    
            if(employee is null)
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
    }
}
