using EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Employees;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Requests
{
    public class PendingRequestDetailsViewModel : PendingRequestBaseModel
    {
        public EmployeeExtendedViewModel Employee { get; init; } = null!;
    }
}
