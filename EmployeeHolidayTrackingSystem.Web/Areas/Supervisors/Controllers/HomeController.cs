using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Services.Requests;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Services.Supervisors;
using EmployeeHolidayTrackingSystem.Web.Infrastructure;
using EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models;
using EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Requests;
using EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Employees;

using static EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.SupervisorConstants;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Controllers
{
    [Area(SupervisorsAreaName)]
    [Authorize(Roles = SupervisorRoleName)]
    public class HomeController : Controller
    {
        private readonly IEmployeeService employees;
        private readonly ISupervisorService supervisors;
        private readonly IRequestService requests;

        public HomeController(IEmployeeService employees,
            ISupervisorService supervisors, IRequestService requests)
        {
            this.employees = employees;
            this.supervisors = supervisors;
            this.requests = requests;
        }

        public async Task<IActionResult> Index()
        {
            var supervisorId = await this.supervisors.GetSupervisorIdByUserIdAsync(this.User.Id()!);

            var employees = await this.employees.GetSupervisorEmployeesAsync(supervisorId);

            var employeesModel = employees
                .Select(e => new EmployeeViewModel()
                {
                    Id = e.Id,
                    FullName = e.FullName,
                }).ToList();

            var requests = await this.requests.GetPendingSupervisorRequestsAsync(supervisorId);
            var requestsModel = requests.Select(r => new PendingRequestViewModel()
            {
                Id = r.Id,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                EmployeeFullName = r.EmployeeFullName
            });

            var model = new SupervisorProfileViewModel()
            {
                FullName = await this.supervisors.GetSupervisorFullNameAsync(supervisorId),
                Employees = employeesModel,
                PendingRequests = requestsModel
            };

            return View(model);
        }
    }
}
