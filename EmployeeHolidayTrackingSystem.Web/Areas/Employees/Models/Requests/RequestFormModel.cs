using System.ComponentModel.DataAnnotations;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models.Requests
{
    public class RequestFormModel
    {
        [Required]
        [Display(Name = "Start Date")]
        public string StartDate { get; set; } = null!;

        [Required]
        [Display(Name = "End Date")]
        public string EndDate { get; set; } = null!;
    }
}