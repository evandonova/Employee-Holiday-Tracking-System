using EmployeeHolidayTrackingSystem.Data;

namespace EmployeeHolidayTrackingSystem.Services.RequestStatuses
{
    public class RequestStatusService : IRequestStatusService
    {

        private readonly EmployeeHolidayDbContext data;

        public RequestStatusService(EmployeeHolidayDbContext data)
            => this.data = data;

        public string? GetTitleById(int statusId)
            => this.data.HolidayRequestStatuses.Find(statusId)?.Title;

        public int GetPendingStatusId()
            => this.data.HolidayRequestStatuses
                    .First(s => s.Title == RequestStatusEnum.Pending.ToString())
                    .Id;
        public int GetApprovedStatusId()
            => this.data.HolidayRequestStatuses
                    .First(s => s.Title == RequestStatusEnum.Approved.ToString())
                    .Id;

        public int GetDisapprovedStatusId()
            => this.data.HolidayRequestStatuses
                    .First(s => s.Title == RequestStatusEnum.Disapproved.ToString())
                    .Id;
    }
}
