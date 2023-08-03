using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Data;
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

        public async Task<IActionResult> Index()
        {
            var employee = await this.employees.GetEmployeeProfileDataAsync(this.User.Id()!);

            var requests = await this.requests.GetEmployeeRequestsAsync(employee.Id);

            var requestsModel = requests
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
                FullName = employee.FullName,
                SupervisorName = employee.SupervisorName,
                HolidayDaysRemaining = employee.HolidayDaysRemaining,
                PendingHolidayRequests = requestsModel.Where(r => r.Status == RequestStatusEnum.Pending.ToString()),
                ApprovedHolidayRequests = requestsModel.Where(r => r.Status == RequestStatusEnum.Approved.ToString()),
                DisapprovedHolidayRequests = requestsModel.Where(r => r.Status == RequestStatusEnum.Disapproved.ToString())
            };

            return View(model);
        }
    }
}
