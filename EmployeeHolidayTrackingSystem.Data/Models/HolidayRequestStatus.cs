using System.ComponentModel.DataAnnotations;

using static EmployeeHolidayTrackingSystem.Data.DataConstants.HolidayRequestStatus;

namespace EmployeeHolidayTrackingSystem.Data.Models
{
    public class HolidayRequestStatus
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; init; } = null!;

        public IEnumerable<HolidayRequest> HolidayRequests { get; init; } = new List<HolidayRequest>();
    }
}
