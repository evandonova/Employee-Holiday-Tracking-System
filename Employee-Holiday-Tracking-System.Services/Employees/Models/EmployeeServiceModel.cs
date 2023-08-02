namespace EmployeeHolidayTrackingSystem.Services.Employees.Models
{
    public class EmployeeServiceModel
    {
        public Guid Id { get; init; }

        public string? FullName { get; init; }

        public int HolidayDaysRemaining { get; init; }

        public string? SupervisorName { get; init; }
    }
}
