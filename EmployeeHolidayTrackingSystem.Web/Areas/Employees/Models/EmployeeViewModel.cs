using EmployeeHolidayTrackingSystem.Web.Models.HolidayRequests;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models
{
    public class EmployeeViewModel
    {
        public string FullName { get; init; } = null!;

        public int HolidayDaysRemaining { get; init; }

        public string SupervisorName { get; init; } = null!;

        public IEnumerable<HolidayRequestViewModel> PendingHolidayRequests { get; init; } 
            = new List<HolidayRequestViewModel>();

        public IEnumerable<HolidayRequestViewModel> ApprovedHolidayRequests { get; init; }
            = new List<HolidayRequestViewModel>();

        public IEnumerable<HolidayRequestViewModel> DisapprovedHolidayRequests { get; init; }
            = new List<HolidayRequestViewModel>();
    }
}
