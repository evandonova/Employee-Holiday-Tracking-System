using System.ComponentModel.DataAnnotations;
using EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models.Requests;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models
{
    public class EmployeeRequestViewModel : RequestViewModel
    {
        [Display(Name = "Disapproval Statement")]
        public string? DisapprovalStatement { get; init; }
    }
}
