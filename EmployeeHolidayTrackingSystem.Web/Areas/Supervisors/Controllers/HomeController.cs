using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Web.Infrastructure;
using EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models;
using EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Requests;
using EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Employees;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Services.Supervisors;
using EmployeeHolidayTrackingSystem.Services.RequestStatuses;

using static EmployeeHolidayTrackingSystem.Web.Constants;
using static EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.SupervisorConstants;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Controllers
{
    [Area(SupervisorsAreaName)]
    [Authorize(Roles = SupervisorRoleName)]
    public class HomeController : Controller
    {
        private readonly IEmployeeService employees;
        private readonly ISupervisorService supervisors;
        private readonly IRequestStatusService statuses;

        public HomeController(IEmployeeService employees, 
            ISupervisorService supervisors, IRequestStatusService statuses)
        {
            this.employees = employees;
            this.supervisors = supervisors;
            this.statuses = statuses;
        }

        public IActionResult Index()
        {
            var supervisor = this.supervisors.GetSupervisorByUserId(this.User.Id());

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
                        StartDate = r.StartDate.ToString(DateFormat),
                        EndDate = r.EndDate.ToString(DateFormat),
                        EmployeeFullName = this.employees.GetEmployeeFullName(r.EmployeeId)
                    })
            };

            return View(model);
        }
    }
}
