using EmployeeHolidayTrackingSystem.Services.Employees.Models;

namespace EmployeeHolidayTrackingSystem.Services.Employees
{
    public interface IEmployeeService
    {
        Task<string> GetEmployeeIdByUserIdAsync(string userId);

        Task<bool> EmployeeExistsAsync(string employeeId);

        Task<string> GetEmployeeEmailAsync(string employeeId);

        Task<string> GetEmployeeFullNameAsync(string employeeId);

        Task<string> GetEmployeeSupervisorIdAsync(string employeeId);

        Task<int> GetEmployeeHolidayDaysRemainingAsync(string employeeId);

        Task<bool> CheckIfEmployeeHasEnoughHolidayDaysAsync(string employeeId, int days);

        Task<EmployeeServiceModel> GetEmployeeProfileDataAsync(string userId);

        Task<EmployeeDetailsServiceModel> GetEmployeeDetailsAsync(string employeeId);

        Task<List<EmployeeServiceModel>> GetSupervisorEmployeesAsync(string supervisorId);

        Task CreateEmployeeAsync(string firstName, string lastName,
            string email, string password, string supervisorId, string employeeRoleName);

        Task SubtractEmployeeHolidayDaysAsync(string supervisorId, int days);

        Task EditEmployeeAsync(string employeeId, string firstName, string lastName,
            string email, string? newPassword);

        Task DeleteEmployeeAsync(string employeeId);

        Task DeleteEmployeesBySupervisorIdAsync(string supervisorId);
    }
}
