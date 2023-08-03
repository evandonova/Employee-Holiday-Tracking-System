using System.ComponentModel.DataAnnotations;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Admin.Models.Supervisors
{
    public class SupervisorViewModel
    {
        public string Id { get; init; } = null!;

        [Display(Name = "Full Name")]
        public string FullName { get; init; } = null!;
    }
}
