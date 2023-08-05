using System.ComponentModel.DataAnnotations;

namespace EmployeeHolidayTrackingSystem.Data.Models
{
    public class Supervisor
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        [Required]
        public string UserId { get; init; } = null!;

        public User User { get; init; } = null!;

        public IEnumerable<Employee> Employees { get; set; } = new List<Employee>();

        public IEnumerable<HolidayRequest> HolidayRequests { get; set; } = new List<HolidayRequest>();
    }
}
