namespace EmployeeHolidayTrackingSystem.Services.Employees.Models
{
    public class EmployeeDetailsServiceModel
    {
        public Guid Id { get; init; }

        public string? FirstName { get; init; }

        public string? LastName { get; init; }

        public string? Email { get; init; }

        public int HolidayDaysRemaining { get; init; }
    }
}
