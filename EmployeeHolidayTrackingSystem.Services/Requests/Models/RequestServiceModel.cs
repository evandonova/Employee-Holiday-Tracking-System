namespace EmployeeHolidayTrackingSystem.Services.Requests.Models
{
    public class RequestServiceModel
    {
        public string Id { get; init; } = null!;

        public string StartDate { get; init; } = null!;

        public string EndDate { get; init; } = null!;

        public string Status { get; init; } = null!;

        public string EmployeeFullName { get; init; } = null!;

        public string? DisapprovalStatement { get; init; }
    }
}
