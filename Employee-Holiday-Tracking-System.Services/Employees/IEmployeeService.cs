using EmployeeHolidayTrackingSystem.Data.Models;

namespace EmployeeHolidayTrackingSystem.Services.Employees
{
    public interface IEmployeeService
    {
        public Employee GetEmployee(string? userId);
    }
}
