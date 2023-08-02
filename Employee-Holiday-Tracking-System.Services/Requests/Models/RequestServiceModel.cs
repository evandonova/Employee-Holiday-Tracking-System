namespace EmployeeHolidayTrackingSystem.Services.Requests.Models
{
    public class RequestServiceModel
    {
        public Guid Id { get; init; }

        public string? StartDate { get; init; }

        public string? EndDate { get; init; }

        public string? Status { get; init; }

        public string? EmployeeFullName { get; init; }

        public string? DisapprovalStatement { get; init; }
    }
}
