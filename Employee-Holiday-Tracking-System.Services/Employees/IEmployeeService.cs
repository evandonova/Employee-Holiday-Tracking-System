using EmployeeHolidayTrackingSystem.Data.Models;

namespace EmployeeHolidayTrackingSystem.Services.Employees
{
    public interface IEmployeeService
    {
        public Employee? GetEmployeeByUserId(string? userId);

        public Employee? GetEmployeeById(Guid id);

        public string GetEmployeeFullName(Guid id);

        public int? GetEmployeeHolidayDaysRemaining(Guid id);

        public bool CheckIfEmployeeHasEnoughHolidayDays(Guid id, int days);

        public void SubtractEmployeeHolidayDays(Guid id, int days);

        public void EditEmployee(Guid id, string firstName, string lastName, 
            string email, string? newPassword);

        public void DeleteEmployee(Guid id);
    }
}
