using EmployeeHolidayTrackingSystem.Data.Models;
using EmployeeHolidayTrackingSystem.Services.Employees.Models;

namespace EmployeeHolidayTrackingSystem.Services.Employees
{
    public interface IEmployeeService
    {
        public Guid GetEmployeeIdByUserId(string userId);

        public bool EmployeeExists(Guid id);

        public string? GetEmployeeEmail(Guid id);

        public string GetEmployeeFullName(Guid id);

        public EmployeeDetailsServiceModel GetEmployeeDetails(Guid id);

        public Guid GetEmployeeSupervisorId(Guid employeeId);

        public List<EmployeeServiceModel> GetSupervisorEmployees(Guid? supervisorId);

        public int? GetEmployeeHolidayDaysRemaining(Guid id);

        public bool CheckIfEmployeeHasEnoughHolidayDays(Guid? id, int days);

        public void CreateEmployee(string firstName, string lastName,
            string email, string password, Guid supervisorId, string employeeRoleName);

        public void SubtractEmployeeHolidayDays(Guid id, int days);

        public void EditEmployee(Guid id, string firstName, string lastName, 
            string email, string? newPassword);

        public void DeleteEmployee(Guid id);

        public void DeleteSupervisorEmployees(Guid supervisorId);

        public EmployeeServiceModel? GetEmployeeProfileData(string? userId);
    }
}
