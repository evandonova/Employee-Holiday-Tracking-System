using Microsoft.EntityFrameworkCore;
using EmployeeHolidayTrackingSystem.Data;

namespace EmployeeHolidayTrackingSystem.Services.RequestStatuses
{
    public class RequestStatusService : IRequestStatusService
    {
        private readonly EmployeeHolidayDbContext data;

        public RequestStatusService(EmployeeHolidayDbContext data)
            => this.data = data;

        public async Task<int> GetPendingStatusIdAsync()
        {
            var status = await this.data.HolidayRequestStatuses
                    .FirstAsync(s => s.Title == RequestStatusEnum.Pending.ToString());
            return status.Id;
        }

        public async Task<int> GetApprovedStatusIdAsync()
        {
            var status = await this.data.HolidayRequestStatuses
                    .FirstAsync(s => s.Title == RequestStatusEnum.Approved.ToString());
            return status.Id;
        }

        public async Task<int> GetDisapprovedStatusIdAsync()
        {
            var status = await this.data.HolidayRequestStatuses
                    .FirstAsync(s => s.Title == RequestStatusEnum.Disapproved.ToString());
            return status.Id;
        }
    }
}
