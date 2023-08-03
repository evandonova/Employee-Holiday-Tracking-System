using System.Globalization;
using Microsoft.EntityFrameworkCore;
using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;
using EmployeeHolidayTrackingSystem.Services.Requests.Models;
using EmployeeHolidayTrackingSystem.Services.RequestStatuses;

using static EmployeeHolidayTrackingSystem.Data.DataConstants.HolidayRequest;

namespace EmployeeHolidayTrackingSystem.Services.Requests
{
    public class RequestService : IRequestService
    {
        private readonly EmployeeHolidayDbContext data;
        private readonly IRequestStatusService statuses;

        public RequestService(EmployeeHolidayDbContext data, IRequestStatusService statuses)
        {
            this.data = data;
            this.statuses = statuses;
        }

        public async Task<string> GetRequestEmployeeIdAsync(string requestId)
        {
            var request = await this.data.HolidayRequests.FirstAsync(r => r.Id.ToString() == requestId);
            return request.EmployeeId.ToString();
        }

        public async Task<bool> RequestExistsAsync(string requestId)
            => await this.data.HolidayRequests.AnyAsync(r => r.Id.ToString() == requestId);

        public async Task CreateAsync(DateTime startDate, DateTime endDate, string employeeId, string supervisorId)
        {
            var request = new HolidayRequest()
            {
                StartDate = startDate,
                EndDate = endDate,
                StatusId = await this.statuses.GetPendingStatusIdAsync(),
                EmployeeId = Guid.Parse(employeeId),
                SupervisorId = Guid.Parse(supervisorId),
            };

            await this.data.HolidayRequests.AddAsync(request);
            await this.data.SaveChangesAsync();
        }

        public async Task UpdateRequestToApprovedAsync(string requestId)
        {
            var request = await this.data.HolidayRequests.FirstAsync(r => r.Id.ToString() == requestId);

            request.StatusId = await this.statuses.GetApprovedStatusIdAsync();

            await this.data.SaveChangesAsync();
        }

        public async Task UpdateDisapprovedRequestAsync(string requestId, string statement)
        {
            var request = await this.data.HolidayRequests.FirstAsync(r => r.Id.ToString() == requestId);

            request.StatusId = await this.statuses.GetDisapprovedStatusIdAsync();
            request.DisapprovalStatement = statement;

            await this.data.SaveChangesAsync();
        }

        public async Task DeleteEmployeeRequestsAsync(string employeeId)
        {
            var requests = await this.data.HolidayRequests
                .Where(r => r.EmployeeId.ToString() == employeeId)
                .ToListAsync();

            this.data.HolidayRequests.RemoveRange(requests);
            await this.data.SaveChangesAsync();
        }

        public async Task<RequestServiceModel> GetRequestDetailsAsync(string requestId)
            => await this.data.HolidayRequests
                 .Where(r => r.Id.ToString() == requestId)
                 .Select(r => new RequestServiceModel()
                 {
                     Id = r.Id.ToString(),
                     StartDate = r.StartDate.ToString(DateFormat, CultureInfo.InvariantCulture),
                     EndDate = r.EndDate.ToString(DateFormat, CultureInfo.InvariantCulture),
                     DisapprovalStatement = r.DisapprovalStatement,
                     Status = r.Status.Title,
                     EmployeeFullName = $"{r.Employee.User.FirstName} {r.Employee.User.LastName}"
                 })
                .FirstAsync();

        public async Task<List<RequestServiceModel>> GetEmployeeRequestsAsync(string employeeId)
            => await this.data.HolidayRequests
                .Where(h => h.EmployeeId.ToString() == employeeId)
                .Select(h => new RequestServiceModel()
                {
                    Id = h.Id.ToString(),
                    StartDate = h.StartDate.ToString(DateFormat, CultureInfo.InvariantCulture),
                    EndDate = h.EndDate.ToString(DateFormat, CultureInfo.InvariantCulture),
                    Status = h.Status.Title
                })
                .ToListAsync();

        public async Task<List<RequestServiceModel>> GetPendingSupervisorRequestsAsync(string supervisorId)
            => await this.data.HolidayRequests
                .Where(r => r.SupervisorId.ToString() == supervisorId 
                    && r.Status.Title == RequestStatusEnum.Pending.ToString())
                .Select(r => new RequestServiceModel()
                {
                    Id = r.Id.ToString(),
                    EmployeeFullName = $"{r.Employee.User.FirstName} {r.Employee.User.LastName}",
                    StartDate = r.StartDate.ToString(DateFormat, CultureInfo.InvariantCulture),
                    EndDate = r.EndDate.ToString(DateFormat, CultureInfo.InvariantCulture)
                })
                .ToListAsync();
    }
}
