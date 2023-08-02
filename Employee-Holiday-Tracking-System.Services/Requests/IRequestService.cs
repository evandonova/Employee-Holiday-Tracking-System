using EmployeeHolidayTrackingSystem.Services.Requests.Models;

namespace EmployeeHolidayTrackingSystem.Services.Requests
{
    public interface IRequestService
    {
        public bool RequestExists(Guid id);

        public void Create(DateTime startDate, DateTime endDate, Guid employeeId, Guid supervisorId);

        public void UpdateRequestToApproved(Guid requestId);

        public void UpdateDisapprovedRequest(Guid requestId, string statement);

        public void DeleteEmployeeRequests(Guid employeeId);

        public List<RequestServiceModel> GetPendingEmployeeRequests(Guid? employeeId);

        public List<RequestServiceModel> GetApprovedEmployeeRequests(Guid? employeeId);

        public List<RequestServiceModel> GetDisapprovedEmployeeRequests(Guid? employeeId);

        public RequestServiceModel? GetRequestDetails(Guid id);

        public Guid GetRequestEmployeeId(Guid id);

        public List<RequestServiceModel> GetPendingSupervisorRequests(Guid? supervisorId);
    }
}
