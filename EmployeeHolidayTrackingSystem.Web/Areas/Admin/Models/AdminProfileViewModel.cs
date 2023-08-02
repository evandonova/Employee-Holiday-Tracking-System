using EmployeeHolidayTrackingSystem.Web.Areas.Admin.Models.Supervisors;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Admin.Models
{
    public class AdminProfileViewModel
    {
        public string FullName { get; init; } = null!;

        public IEnumerable<SupervisorViewModel> Supervisors { get; init; }
            = new List<SupervisorViewModel>();
    }
}
