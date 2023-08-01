using System.ComponentModel.DataAnnotations;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models
{
    public class EmployeeViewModel
    {
        public Guid Id { get; init; }

        [Display(Name = "Full Name")]
        public string FullName { get; init; } = null!;
    }
}
