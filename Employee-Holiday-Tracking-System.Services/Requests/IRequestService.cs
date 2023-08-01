using EmployeeHolidayTrackingSystem.Data.Models;

namespace EmployeeHolidayTrackingSystem.Services.Requests
{
    public interface IRequestService
    {
        public HolidayRequest? GetById(Guid id);

        public void Create(DateTime startDate, DateTime endDate, Guid employeeId, Guid supervisorId);
    }
}
