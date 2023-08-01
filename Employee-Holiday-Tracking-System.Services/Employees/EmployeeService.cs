using Microsoft.EntityFrameworkCore;
using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;

namespace EmployeeHolidayTrackingSystem.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeHolidayDbContext data;

        public EmployeeService(EmployeeHolidayDbContext data)
            => this.data = data;

        public Employee? GetEmployeeByUserId(string? userId)
            => this.data.Employees
                    .Include(e => e.User)
                    .Include(e => e.Supervisor.User)
                    .Include(e => e.HolidayRequests)
                    .FirstOrDefault(e => e.UserId == userId);

        public Employee? GetEmployeeById(Guid id)
            => this.data.Employees
                    .Find(id);

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
    }
}
