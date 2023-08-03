using EmployeeHolidayTrackingSystem.Services.Requests.Models;

namespace EmployeeHolidayTrackingSystem.Services.Requests
{
    public interface IRequestService
    {
        public Task<string> GetRequestEmployeeIdAsync(string requestId);

        public Task<bool> RequestExistsAsync(string requestId);

        public Task CreateAsync(DateTime startDate, DateTime endDate, string employeeId, string supervisorId);

        public Task UpdateRequestToApprovedAsync(string requestId);

        public Task UpdateDisapprovedRequestAsync(string requestId, string statement);

        public Task DeleteEmployeeRequestsAsync(string employeeId);

        public Task<RequestServiceModel> GetRequestDetailsAsync(string requestId);

        public Task<List<RequestServiceModel>> GetEmployeeRequestsAsync(string employeeId);

        public Task<List<RequestServiceModel>> GetPendingSupervisorRequestsAsync(string supervisorId);
    }
}
