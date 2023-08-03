using System.ComponentModel.DataAnnotations;

using static EmployeeHolidayTrackingSystem.Data.DataConstants.User;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Shared.Models
{
    public class UserBaseFormModel
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        public string FirstName { get; init; } = null!;

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        public string LastName { get; init; } = null!;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength)]
        public string Email { get; init; } = null!;
    }
}
