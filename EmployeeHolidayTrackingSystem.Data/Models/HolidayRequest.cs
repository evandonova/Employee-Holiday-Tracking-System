using System.ComponentModel.DataAnnotations;

using static EmployeeHolidayTrackingSystem.Data.DataConstants.HolidayRequest;

namespace EmployeeHolidayTrackingSystem.Data.Models
{
    public class HolidayRequest
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int StatusId { get; set; }

        public HolidayRequestStatus Status { get; set; } = null!;

        [MaxLength(DisapprovalStatementMaxLength)]
        public string? DisapprovalStatement { get; set; }

        public Guid EmployeeId { get; init; }

        public Employee Employee { get; init; } = null!;

        public Guid SupervisorId { get; init; }

        public Supervisor Supervisor { get; init; } = null!;
    }
}
