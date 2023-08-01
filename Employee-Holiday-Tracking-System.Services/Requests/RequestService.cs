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

        public HolidayRequest? GetById(Guid id)
            => this.data.HolidayRequests.Find(id);

        public void Create(DateTime startDate, DateTime endDate, Guid employeeId, Guid supervisorId)
        {
            var request = new HolidayRequest()
            {
                StartDate = startDate,
                EndDate = endDate,
                StatusId = statuses.GetPendingStatusId(),
                EmployeeId = employeeId,
                SupervisorId = supervisorId,
            };

            data.HolidayRequests.Add(request);
            data.SaveChanges();
        }

    }
}
