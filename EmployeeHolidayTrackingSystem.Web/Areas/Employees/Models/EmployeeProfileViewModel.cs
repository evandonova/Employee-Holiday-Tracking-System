using System.ComponentModel.DataAnnotations;
using EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models.Requests;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models
{
    public class EmployeeProfileViewModel
    {
        [Display(Name = "Full Name")]
        public string FullName { get; init; } = null!;

        [Display(Name = "Holiday Days Remaining")]
        public int HolidayDaysRemaining { get; init; }

        [Display(Name = "Supervisor Name")]
        public string SupervisorName { get; init; } = null!;

        public IEnumerable<RequestViewModel> PendingHolidayRequests { get; init; }
            = new List<RequestViewModel>();

        public IEnumerable<RequestViewModel> ApprovedHolidayRequests { get; init; }
            = new List<RequestViewModel>();

        public IEnumerable<RequestViewModel> DisapprovedHolidayRequests { get; init; }
            = new List<RequestViewModel>();
    }
}
