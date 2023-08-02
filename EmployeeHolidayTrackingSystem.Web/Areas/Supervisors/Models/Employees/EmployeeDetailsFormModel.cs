using System.ComponentModel.DataAnnotations;

using static EmployeeHolidayTrackingSystem.Data.DataConstants.User;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Employees
{
    public class EmployeeDetailsFormModel
    {
        public Guid Id { get; init; }

        [Required]
        [Display (Name = "First Name")]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        public string? FirstName { get; init; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        public string? LastName { get; init; }

        [Display(Name = "Holiday Days Remaining")]
        public int HolidayDaysRemaining { get; init; }

        [Required]
        [EmailAddress]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength)]
        public string? Email { get; init; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
        public string? NewPassword { get; init; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; init; }
    }
}
