namespace EmployeeHolidayTrackingSystem.Services.RequestStatuses
{
    public interface IRequestStatusService
    {
        Task<int> GetPendingStatusIdAsync();

        Task<int> GetApprovedStatusIdAsync();

        Task<int> GetDisapprovedStatusIdAsync();
    }
}
