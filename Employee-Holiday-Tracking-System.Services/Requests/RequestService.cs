using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;
using EmployeeHolidayTrackingSystem.Services.RequestStatuses;

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

        public HolidayRequest? GetRequestById(Guid id)
            => this.data.HolidayRequests.Find(id);

        public bool RequestExists(Guid id)
            => this.data.HolidayRequests.Find(id) is not null;

        public void Create(DateTime startDate, DateTime endDate, Guid employeeId, Guid supervisorId)
        {
            var request = new HolidayRequest()
            {
                StartDate = startDate,
                EndDate = endDate,
                StatusId = this.statuses.GetPendingStatusId(),
                EmployeeId = employeeId,
                SupervisorId = supervisorId,
            };

            data.HolidayRequests.Add(request);
            data.SaveChanges();
        }

        public void UpdateRequestToApproved(Guid requestId)
        {
            var request = this.GetRequestById(requestId);

            if(request is null) 
            {
                return;
            }

            request.StatusId = this.statuses.GetApprovedStatusId();
            data.SaveChanges();
        }

        public void UpdateDisapprovedRequest(Guid requestId, string statement)
        {
            var request = this.GetRequestById(requestId);

            if (request is null)
            {
                return;
            }

            request.StatusId = this.statuses.GetDisapprovedStatusId();
            request.DisapprovalStatement = statement;

            data.SaveChanges();
        }
    }
}
