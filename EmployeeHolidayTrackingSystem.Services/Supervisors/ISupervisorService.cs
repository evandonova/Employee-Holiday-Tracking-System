using EmployeeHolidayTrackingSystem.Services.Supervisors.Models;

namespace EmployeeHolidayTrackingSystem.Services.Supervisors
{
    public interface ISupervisorService
    {
        Task<string> GetSupervisorIdByUserIdAsync(string userId);

        Task<string> GetSupervisorFullNameAsync(string supervisorId);

        Task<bool> SupervisorExistsAsync(string supervisorId);

        Task<string> GetSupervisorEmailAsync(string supervisorId);

        Task<SupervisorDetailsServiceModel> GetSupervisorDetailsAsync(string supervisorId);

        Task<List<SupervisorServiceModel>> GetAllSupervisorsAsync();

        Task CreateSupervisorAsync(string firstName, string lastName,
            string email, string password, string employeeRoleName);

        Task EditSupervisorAsync(string id, string firstName, string lastName,
            string email, string? newPassword);

        Task DeleteSupervisorAsync(string id);
    }
}
