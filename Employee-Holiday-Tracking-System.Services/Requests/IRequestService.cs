using EmployeeHolidayTrackingSystem.Data.Models;

namespace EmployeeHolidayTrackingSystem.Services.Requests
{
    public interface IRequestService
    {
        public HolidayRequest? GetById(Guid id);

        public bool Exists(Guid id);

        public void Create(DateTime startDate, DateTime endDate, Guid employeeId, Guid supervisorId);

        void ChangeRequestStatusToApproved(Guid requestId);

        public void UpdateDisapprovedRequest(Guid requestId, string statement);
    }
}
