using System.ComponentModel.DataAnnotations;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Requests
{
    public class PendingRequestBaseModel
    {
        public string Id { get; init; } = null!;

        [Display(Name = "Start Date")]
        public string StartDate { get; init; } = null!;

        [Display(Name = "End Date")]
        public string EndDate { get; init; } = null!;
    }
}
