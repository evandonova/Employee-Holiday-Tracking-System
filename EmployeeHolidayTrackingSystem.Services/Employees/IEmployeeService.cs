using EmployeeHolidayTrackingSystem.Services.Employees.Models;

namespace EmployeeHolidayTrackingSystem.Services.Employees
{
    public interface IEmployeeService
    {
        public Task<string> GetEmployeeIdByUserIdAsync(string userId);

        public Task<bool> EmployeeExistsAsync(string employeeId);

        public Task<string> GetEmployeeEmailAsync(string employeeId);

        public Task<string> GetEmployeeFullNameAsync(string employeeId);

        public Task<string> GetEmployeeSupervisorIdAsync(string employeeId);

        public Task<int> GetEmployeeHolidayDaysRemainingAsync(string employeeId);

        public Task<bool> CheckIfEmployeeHasEnoughHolidayDaysAsync(string employeeId, int days);

        public Task<EmployeeServiceModel> GetEmployeeProfileDataAsync(string userId);

        public Task<EmployeeDetailsServiceModel> GetEmployeeDetailsAsync(string employeeId);

        public Task<List<EmployeeServiceModel>> GetSupervisorEmployeesAsync(string supervisorId);

        public Task CreateEmployeeAsync(string firstName, string lastName,
            string email, string password, string supervisorId, string employeeRoleName);

        public Task SubtractEmployeeHolidayDaysAsync(string supervisorId, int days);

        public Task EditEmployeeAsync(string employeeId, string firstName, string lastName, 
            string email, string? newPassword);

        public Task DeleteEmployeeAsync(string employeeId);

        public Task DeleteEmployeesBySupervisorIdAsync(string supervisorId);
    }
}
