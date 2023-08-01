using EmployeeHolidayTrackingSystem.Web.Models.Requests;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models
{
    public class PendingExtendedRequestViewModel : RequestViewModel
    {
        public EmployeeExtendedViewModel Employee { get; init; } = null!;
    }
}
