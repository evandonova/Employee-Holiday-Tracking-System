using EmployeeHolidayTrackingSystem.Web.Models.Requests;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models
{
    public class EmployeeViewModel
    {
        public string FullName { get; init; } = null!;

        public int HolidayDaysRemaining { get; init; }

        public string SupervisorName { get; init; } = null!;

        public IEnumerable<RequestViewModel> PendingHolidayRequests { get; init; } 
            = new List<RequestViewModel>();

        public IEnumerable<RequestViewModel> ApprovedHolidayRequests { get; init; }
            = new List<RequestViewModel>();

        public IEnumerable<RequestViewModel> DisapprovedHolidayRequests { get; init; }
            = new List<RequestViewModel>();
    }
}
