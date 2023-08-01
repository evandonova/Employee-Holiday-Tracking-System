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

        public Employee GetEmployee(string? userId)
            => this.data.Employees
                    .Include(e => e.User)
                    .Include(e => e.Supervisor.User)
                    .Include(e => e.HolidayRequests)
                    .First(e => e.UserId == userId);
    }
}
