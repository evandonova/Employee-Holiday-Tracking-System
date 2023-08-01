namespace EmployeeHolidayTrackingSystem.Services.RequestStatuses
{
    public interface IRequestStatusService
    {
        public string? GetStatusTitleById(int statusId);

        public int GetPendingStatusId();

        public int GetApprovedStatusId();

        public int GetDisapprovedStatusId();
    }
}
