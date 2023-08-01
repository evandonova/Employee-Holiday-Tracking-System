using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Web.Infrastructure;
using EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models;
using EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Requests;
using EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Employees;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Services.Supervisors;
using EmployeeHolidayTrackingSystem.Services.RequestStatuses;

using static EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.SupervisorConstants;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Controllers
{
    [Area(SupervisorsAreaName)]
    [Authorize(Roles = SupervisorRoleName)]
    public class HomeController : Controller
    {
        private readonly ISupervisorService supervisors;
        private readonly IEmployeeService employees;
        private readonly IRequestStatusService statuses;

        public HomeController(ISupervisorService supervisors, 
            IEmployeeService employees, IRequestStatusService statuses)
        {
            this.supervisors = supervisors;
            this.employees = employees;
            this.statuses = statuses;
        }

        public IActionResult Index()
        {
            var supervisor = this.supervisors.GetSupervisor(this.User.Id());

            var pendingStatusId = this.statuses.GetPendingStatusId();

            var model = new SupervisorProfileViewModel()
            {
                FullName = $"{supervisor.User.FirstName} {supervisor.User.LastName}",
                Employees = supervisor.Employees
                    .Select(e => new EmployeeViewModel()
                    {
                        Id = e.Id,
                        FullName = this.employees.GetEmployeeFullName(e.Id)
                    }),
                PendingRequests = supervisor.HolidayRequests
                    .Where(r => r.StatusId == pendingStatusId)
                    .Select(r => new PendingRequestViewModel()
                    {
                        Id = r.Id,
                        StartDate = r.StartDate.ToString("d MMMM yyyy"),
                        EndDate = r.EndDate.ToString("d MMMM yyyy"),
                        EmployeeFullName = this.employees.GetEmployeeFullName(r.EmployeeId)
                    })
            };

            return View(model);
        }
    }
}
