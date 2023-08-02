using EmployeeHolidayTrackingSystem.Data.Models;

namespace EmployeeHolidayTrackingSystem.Services.Supervisors
{
    public interface ISupervisorService
    {
        public Supervisor? GetSupervisorById(Guid id);

        public Supervisor? GetSupervisorByUserId(string? userId);

        public List<Supervisor> GetAll();

        public void CreateSupervisor(string firstName, string lastName,
            string email, string password, string employeeRoleName);

        public void EditSupervisor(Guid id, string firstName, string lastName,
            string email, string? newPassword);

        public void DeleteSupervisor(Guid id);
    }
}
