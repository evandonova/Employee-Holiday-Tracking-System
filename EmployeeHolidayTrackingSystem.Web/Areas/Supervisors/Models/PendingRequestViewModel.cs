using EmployeeHolidayTrackingSystem.Web.Models.Requests;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models
{
    public class PendingRequestViewModel : RequestViewModel
    {
        public string EmployeeFullName { get; init; } = null!;
    }
}
