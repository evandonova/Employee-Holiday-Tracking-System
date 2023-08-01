using EmployeeHolidayTrackingSystem.Data.Models;

namespace EmployeeHolidayTrackingSystem.Services.Supervisors
{
    public interface ISupervisorService
    {
        public Supervisor GetSupervisor(string? userId);
    }
}
