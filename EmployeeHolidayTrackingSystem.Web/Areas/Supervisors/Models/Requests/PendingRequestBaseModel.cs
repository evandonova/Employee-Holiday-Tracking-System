using System.ComponentModel.DataAnnotations;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Requests
{
    public class PendingRequestBaseModel
    {
        public Guid Id { get; init; }

        [Display(Name = "Start Date")]
        public string? StartDate { get; init; }

        [Display(Name = "End Date")]
        public string? EndDate { get; init; }
    }
}
