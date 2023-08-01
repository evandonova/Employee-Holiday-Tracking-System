using Microsoft.EntityFrameworkCore;
using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;
using EmployeeHolidayTrackingSystem.Services.Users;

namespace EmployeeHolidayTrackingSystem.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeHolidayDbContext data;
        private readonly IUserService users;

        public EmployeeService(EmployeeHolidayDbContext data, IUserService users)
        {
            this.data = data;
            this.users = users;
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

        public void EditEmployee(Guid id, string firstName, string lastName, string newPassword)
        {
            var employee = this.data.Employees.Find(id);
    
            if(employee is null)
            {
                return;
            }

            employee.User.FirstName = firstName;
            employee.User.LastName = lastName;

            if(newPassword is not null) 
            {
                users.UpdatePassword(employee.UserId, newPassword);
            }

            this.data.SaveChanges();
        }
    }
}
