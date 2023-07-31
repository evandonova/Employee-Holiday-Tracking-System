using System.ComponentModel.DataAnnotations;

namespace EmployeeHolidayTrackingSystem.Web.Models.HolidayRequests
{
    public class HolidayRequestFormModel
    {
        [Display(Name = "Start Date")]
        public string StartDate { get; set; } = null!;

        [Display(Name = "End Date")]
        public string EndDate { get; set; } = null!;
    }
}