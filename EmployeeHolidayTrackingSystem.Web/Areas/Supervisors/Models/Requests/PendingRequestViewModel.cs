using System.ComponentModel.DataAnnotations;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Requests
{
    public class PendingRequestViewModel : PendingRequestBaseModel
    {
        [Display (Name = "Employee Full Name")]
        public string? EmployeeFullName { get; init; }
    }
}
