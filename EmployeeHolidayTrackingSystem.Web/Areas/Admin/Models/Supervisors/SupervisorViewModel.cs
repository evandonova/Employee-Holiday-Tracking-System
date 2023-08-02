using System.ComponentModel.DataAnnotations;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Admin.Models.Supervisors
{
    public class SupervisorViewModel
    {
        public Guid Id { get; init; }

        [Display(Name = "Full Name")]
        public string FullName { get; init; } = null!;
    }
}
