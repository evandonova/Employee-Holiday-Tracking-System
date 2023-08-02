using EmployeeHolidayTrackingSystem.Data.Models;

namespace EmployeeHolidayTrackingSystem.Services.Requests
{
    public interface IRequestService
    {
        public HolidayRequest? GetRequestById(Guid id);

        public bool RequestExists(Guid id);

        public void Create(DateTime startDate, DateTime endDate, Guid employeeId, Guid supervisorId);

        public void UpdateRequestToApproved(Guid requestId);

        public void UpdateDisapprovedRequest(Guid requestId, string statement);

        public void DeleteEmployeeRequests(Guid employeeId);
    }
}
