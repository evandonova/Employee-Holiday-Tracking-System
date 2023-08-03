using System.ComponentModel.DataAnnotations;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Employees
{
    public class EmployeeViewModel
    {
        public string Id { get; init; } = null!;

        [Display(Name = "Full Name")]
        public string FullName { get; init; } = null!;
    }
}
