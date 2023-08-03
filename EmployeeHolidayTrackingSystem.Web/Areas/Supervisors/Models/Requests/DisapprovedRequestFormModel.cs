using System.ComponentModel.DataAnnotations;

using static EmployeeHolidayTrackingSystem.Data.DataConstants.HolidayRequest;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Requests
{
    public class DisapprovedRequestFormModel
    {
        public string RequestId { get; init; } = null!;

        [Required]
        [StringLength(DisapprovalStatementMaxLength, MinimumLength = DisapprovalStatementMinLength)]
        public string Statement { get; init; } = null!;
    }
}
