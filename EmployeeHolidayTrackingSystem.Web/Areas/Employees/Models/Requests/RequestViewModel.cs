using System.ComponentModel.DataAnnotations;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models.Requests
{
    public class RequestViewModel
    {
        public Guid Id { get; init; }

        [Display(Name = "Start Date")]
        public string StartDate { get; init; } = null!;

        [Display(Name = "End Date")]
        public string EndDate { get; init; } = null!;

        public string Status { get; init; } = null!;
    }
}
