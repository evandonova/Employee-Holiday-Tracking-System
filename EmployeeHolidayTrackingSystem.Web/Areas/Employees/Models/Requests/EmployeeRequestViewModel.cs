using System.ComponentModel.DataAnnotations;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models.Requests
{
    public class EmployeeRequestViewModel : RequestViewModel
    {
        [Display(Name = "Disapproval Statement")]
        public string? DisapprovalStatement { get; init; }
    }
}
