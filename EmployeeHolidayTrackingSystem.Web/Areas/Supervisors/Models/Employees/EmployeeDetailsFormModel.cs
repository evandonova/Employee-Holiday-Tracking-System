using System.ComponentModel.DataAnnotations;
using EmployeeHolidayTrackingSystem.Web.Areas.Shared.Models;

using static EmployeeHolidayTrackingSystem.Data.DataConstants.User;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Employees
{
    public class EmployeeDetailsFormModel : UserDetailsBaseFormModel
    {
        [Display(Name = "Holiday Days Remaining")]
        public int HolidayDaysRemaining { get; init; }
    }
}
