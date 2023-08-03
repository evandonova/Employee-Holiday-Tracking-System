using EmployeeHolidayTrackingSystem.Services.Supervisors.Models;

namespace EmployeeHolidayTrackingSystem.Services.Supervisors
{
    public interface ISupervisorService
    {
        public Task<string> GetSupervisorIdByUserIdAsync(string userId);

        public Task<string> GetSupervisorFullNameAsync(string supervisorId);

        public Task<bool> SupervisorExistsAsync(string supervisorId);

        public Task<string> GetSupervisorEmailAsync(string supervisorId);

        public Task<SupervisorDetailsServiceModel> GetSupervisorDetailsAsync(string supervisorId);

        public Task<List<SupervisorServiceModel>> GetAllSupervisorsAsync();

        public Task CreateSupervisorAsync(string firstName, string lastName,
            string email, string password, string employeeRoleName);

        public Task EditSupervisorAsync(string id, string firstName, string lastName,
            string email, string? newPassword);

        public Task DeleteSupervisorAsync(string id);
    }
}
