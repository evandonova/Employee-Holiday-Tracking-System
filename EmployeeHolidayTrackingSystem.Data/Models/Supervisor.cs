using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EmployeeHolidayTrackingSystem.Data.Models
{
    public class Supervisor
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        [Required]
        public string UserId { get; init; } = null!;

        public IdentityUser User { get; init; } = null!;

        public IEnumerable<Employee> Employees { get; set; } = new List<Employee>();

        public IEnumerable<HolidayRequest> PendingHolidayRequests { get; set; } = new List<HolidayRequest>();
    }
}
