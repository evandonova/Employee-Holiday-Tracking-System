using System.ComponentModel.DataAnnotations;

using static EmployeeHolidayTrackingSystem.Data.DataConstants.User;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Shared.Models
{
    public class UserFormModel : UserBaseFormModel
    {
        [Required]
        [DataType(DataType.Password)]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
        public string Password { get; init; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; init; } = null!;
    }
}
