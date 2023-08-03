namespace EmployeeHolidayTrackingSystem.Services.RequestStatuses
{
    public interface IRequestStatusService
    {
        public Task<int> GetPendingStatusIdAsync();

        public Task<int> GetApprovedStatusIdAsync();

        public Task<int> GetDisapprovedStatusIdAsync();
    }
}
