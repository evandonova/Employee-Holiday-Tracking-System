namespace EmployeeHolidayTrackingSystem.Services.Employees.Models
{
    public class EmployeeDetailsServiceModel
    {
        public string Id { get; init; } = null!;

        public string FirstName { get; init; } = null!;

        public string LastName { get; init; } = null!;

        public string Email { get; init; } = null!;

        public int HolidayDaysRemaining { get; init; }
    }
}
