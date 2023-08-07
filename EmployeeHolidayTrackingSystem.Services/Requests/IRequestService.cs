using EmployeeHolidayTrackingSystem.Services.Requests.Models;

namespace EmployeeHolidayTrackingSystem.Services.Requests
{
    public interface IRequestService
    {
        Task<bool> RequestExistsAsync(string requestId);

        Task<string> GetRequestEmployeeIdAsync(string requestId);

        Task<string> GetRequestEmployeeFullNameAsync(string requestId);

        Task<string> GetRequestStatusTitleAsync(string requestId);

        Task<string?> GetRequestDisapprovalStatementAsync(string requestId);

        Task CreateRequestAsync(DateTime startDate, DateTime endDate, string employeeId, string supervisorId);

        Task UpdateRequestToApprovedAsync(string requestId);

        Task UpdateDisapprovedRequestAsync(string requestId, string statement);

        Task DeleteEmployeeRequestsAsync(string employeeId);

        Task<RequestServiceModel> GetRequestDetailsAsync(string requestId);

        Task<List<RequestServiceModel>> GetEmployeeRequestsAsync(string employeeId);

        Task<List<RequestServiceModel>> GetPendingSupervisorRequestsAsync(string supervisorId);
    }
}
