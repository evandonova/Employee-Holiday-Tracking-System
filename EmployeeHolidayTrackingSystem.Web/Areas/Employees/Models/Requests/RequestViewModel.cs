using System.ComponentModel.DataAnnotations;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models.Requests
{
    public class RequestViewModel : RequestFormModel
    {
        public string Id { get; init; } = null!;

        public string Status { get; init; } = null!;
    }
}
