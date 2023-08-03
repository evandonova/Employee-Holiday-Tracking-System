using EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Employees;
using EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Requests;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models
{
    public class SupervisorProfileViewModel
    {
        public string FullName { get; init; } = null!;

        public IEnumerable<EmployeeViewModel> Employees { get; init; }
            = new List<EmployeeViewModel>();

        public IEnumerable<PendingRequestViewModel> PendingRequests { get; init; } 
            = new List<PendingRequestViewModel>();
    }
}
