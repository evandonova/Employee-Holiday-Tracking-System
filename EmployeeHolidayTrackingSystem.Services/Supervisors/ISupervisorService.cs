using EmployeeHolidayTrackingSystem.Services.Supervisors.Models;

namespace EmployeeHolidayTrackingSystem.Services.Supervisors
{
    public interface ISupervisorService
    {
        public string? GetSupervisorFullName(Guid? supervisorId);

        public Guid GetSupervisorIdByUserId(string userId);

        public SupervisorDetailsServiceModel? GetDetails(Guid id);

        public bool SupervisorExists(Guid id);

        public string? GetSupervisorEmail(Guid id);

        public List<SupervisorServiceModel> GetAll();

        public void CreateSupervisor(string firstName, string lastName,
            string email, string password, string employeeRoleName);

        public void EditSupervisor(Guid id, string firstName, string lastName,
            string email, string? newPassword);

        public void DeleteSupervisor(Guid id);
    }
}
