using EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models.Requests;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models
{
    public class EmployeeRequestViewModel : RequestViewModel
    {
        public string? DisapprovalStatement { get; init; }
    }
}
