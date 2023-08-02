using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Services.Requests;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Web.Infrastructure;
using EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models;
using EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models.Requests;

using static EmployeeHolidayTrackingSystem.Web.Areas.Employees.EmployeeConstants;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Employees.Controllers
{
    [Area(EmployeesAreaName)]
    [Authorize(Roles = EmployeeRoleName)]
    public class HomeController : Controller
    {
        private readonly IRequestService requests;
        private readonly IEmployeeService employees;

        public HomeController(IRequestService requests, IEmployeeService employees)
        {
            this.requests = requests;
            this.employees = employees;
        }

        public IActionResult Index()
        {
            var employee = this.employees.GetEmployeeProfileData(this.User.Id());

            var pendingRequests = this.requests.GetPendingEmployeeRequests(employee?.Id)
                .Select(r => new RequestViewModel()
                {
                    Id = r.Id,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    Status = r.Status
                })
                .ToList();

            var approvedRequests = this.requests.GetApprovedEmployeeRequests(employee?.Id)
                .Select(r => new RequestViewModel()
                {
                    Id = r.Id,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    Status = r.Status
                })
                .ToList();

            var disapprovedRequests = this.requests.GetDisapprovedEmployeeRequests(employee?.Id)
                .Select(r => new RequestViewModel()
                {
                    Id = r.Id,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    Status = r.Status
                })
                .ToList();

            var model = new EmployeeProfileViewModel()
            {
                FullName = employee?.FullName,
                SupervisorName = employee?.SupervisorName,
                HolidayDaysRemaining = employee?.HolidayDaysRemaining ?? 0,
                PendingHolidayRequests = pendingRequests,
                ApprovedHolidayRequests = approvedRequests,
                DisapprovedHolidayRequests = disapprovedRequests
            };

            return View(model);
        }
    }
}
