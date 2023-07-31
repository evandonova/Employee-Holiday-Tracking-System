using System.Security.Claims;

namespace EmployeeHolidayTrackingSystem.Web.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? Id(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
