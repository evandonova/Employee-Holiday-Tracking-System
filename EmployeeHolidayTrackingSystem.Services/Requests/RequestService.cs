using System.Globalization;
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

            if (request is null)
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

        public void DeleteEmployeeRequests(Guid employeeId)
        {
            var requests = this.data.HolidayRequests.Where(r => r.EmployeeId == employeeId);

            this.data.HolidayRequests.RemoveRange(requests);
            this.data.SaveChanges();
        }

        public List<RequestServiceModel> GetPendingEmployeeRequests(Guid? employeeId)
            => this.data.HolidayRequests
                .Where(h => h.EmployeeId == employeeId &&
                    h.Status.Title == RequestStatusEnum.Pending.ToString())
                .Select(h => new RequestServiceModel()
                {
                    Id = h.Id,
                    StartDate = h.StartDate.ToString(DateFormat, CultureInfo.InvariantCulture),
                    EndDate = h.EndDate.ToString(DateFormat, CultureInfo.InvariantCulture),
                    Status = h.Status.Title
                })
                .ToList();

        public List<RequestServiceModel> GetApprovedEmployeeRequests(Guid? employeeId)
        => this.data.HolidayRequests
                .Where(h => h.EmployeeId == employeeId &&
                    h.Status.Title == RequestStatusEnum.Approved.ToString())
                .Select(h => new RequestServiceModel()
                {
                    Id = h.Id,
                    StartDate = h.StartDate.ToString(DateFormat, CultureInfo.InvariantCulture),
                    EndDate = h.EndDate.ToString(DateFormat, CultureInfo.InvariantCulture),
                    Status = h.Status.Title
                })
                .ToList();

        public List<RequestServiceModel> GetDisapprovedEmployeeRequests(Guid? employeeId)
            => this.data.HolidayRequests
                    .Where(h => h.EmployeeId == employeeId &&
                        h.Status.Title == RequestStatusEnum.Disapproved.ToString())
                    .Select(h => new RequestServiceModel()
                    {
                        Id = h.Id,
                        StartDate = h.StartDate.ToString(DateFormat, CultureInfo.InvariantCulture),
                        EndDate = h.EndDate.ToString(DateFormat, CultureInfo.InvariantCulture),
                        Status = h.Status.Title
                    })
                    .ToList();

        public RequestServiceModel? GetRequestDetails(Guid id)
            => this.data.HolidayRequests
                 .Where(r => r.Id == id)
                 .Select(r => new RequestServiceModel()
                 {
                     Id = r.Id,
                     StartDate = r.StartDate.ToString(DateFormat, CultureInfo.InvariantCulture),
                     EndDate = r.EndDate.ToString(DateFormat, CultureInfo.InvariantCulture),
                     DisapprovalStatement = r.DisapprovalStatement,
                     Status = r.Status.Title,
                     EmployeeFullName = $"{r.Employee.User.FirstName} {r.Employee.User.LastName}"
                 })
                .FirstOrDefault();

        public Guid GetRequestEmployeeId(Guid id)
            => this.data.HolidayRequests.Find(id)!.EmployeeId;

        public List<RequestServiceModel> GetPendingSupervisorRequests(Guid? supervisorId)
            => this.data.HolidayRequests
            .Where(r => r.SupervisorId == supervisorId && r.Status.Title == RequestStatusEnum.Pending.ToString())
            .Select(r => new RequestServiceModel()
            {
                Id = r.Id,
                EmployeeFullName = $"{r.Employee.User.FirstName} {r.Employee.User.LastName}",
                StartDate = r.StartDate.ToString(DateFormat, CultureInfo.InvariantCulture),
                EndDate = r.EndDate.ToString(DateFormat, CultureInfo.InvariantCulture)
            })
            .ToList();

        private HolidayRequest? GetRequestById(Guid id)
            => this.data.HolidayRequests.Find(id);
    }
}
