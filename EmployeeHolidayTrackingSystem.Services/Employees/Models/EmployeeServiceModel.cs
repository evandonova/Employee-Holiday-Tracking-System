namespace EmployeeHolidayTrackingSystem.Services.Employees.Models
{
    public class EmployeeServiceModel
    {
        public string Id { get; init; } = null!;

        public string FullName { get; init; } = null!;

        public int HolidayDaysRemaining { get; init; }

        public string SupervisorName { get; init; } = null!;
    }
}
