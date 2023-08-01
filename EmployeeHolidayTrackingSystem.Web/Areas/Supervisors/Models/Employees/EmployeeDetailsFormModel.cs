using System.ComponentModel.DataAnnotations;

using static EmployeeHolidayTrackingSystem.Data.DataConstants.User;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Employees
{
    public class EmployeeDetailsFormModel
    {
        public Guid Id { get; init; }

        [Required]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        public string FirstName { get; init; } = null!;

        [Required]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        public string LastName { get; init; } = null!;

        public int HolidayDaysRemaining { get; init; }

        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
        public string? NewPassword { get; init; }
    }
}
