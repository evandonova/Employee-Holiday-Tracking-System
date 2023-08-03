using System.ComponentModel.DataAnnotations;

using static EmployeeHolidayTrackingSystem.Data.DataConstants.User;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Shared.Models
{
    public class UserDetailsBaseFormModel : UserBaseFormModel
    {
        public string Id { get; init; } = null!;

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
