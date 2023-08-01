using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Web.Infrastructure;
using EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models;
using EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models.Requests;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Services.RequestStatuses;

using static EmployeeHolidayTrackingSystem.Web.Areas.Employees.EmployeeConstants;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Employees.Controllers
{
    [Area(EmployeesAreaName)]
    [Authorize(Roles = EmployeeRoleName)]
    public class HomeController : Controller
    {
        private readonly IEmployeeService employees;
        private readonly IRequestStatusService statuses;

        public HomeController(IEmployeeService employees, IRequestStatusService statuses)
        {
            this.employees = employees;
            this.statuses = statuses;
        }

        public IActionResult Index()
        {
            var employee = employees.GetEmployeeByUserId(this.User.Id());

            var holidayRequests = employee.HolidayRequests
                    .Select(hr => new RequestViewModel
                    {
                        Id = hr.Id,
                        StartDate = hr.StartDate.ToString("d MMMM yyyy"),
                        EndDate = hr.EndDate.ToString("d MMMM yyyy"),
                        Status = statuses.GetTitleById(hr.StatusId) ?? "Pending"
                    })
                    .ToList();

            var supervisor = employee.Supervisor;

            var employeeModel = new EmployeeProfileViewModel()
            {
                FullName = $"{employee.User.FirstName} {employee.User.LastName}",
                SupervisorName = $"{supervisor.User.FirstName} {supervisor.User.LastName}",
                HolidayDaysRemaining = employee.HolidayDaysRemaining,
                PendingHolidayRequests = holidayRequests
                    .Where(h => h.Status == RequestStatusEnum.Pending.ToString()),
                ApprovedHolidayRequests = holidayRequests
                    .Where(h => h.Status == RequestStatusEnum.Approved.ToString()),
                DisapprovedHolidayRequests = holidayRequests
                    .Where(h => h.Status == RequestStatusEnum.Disapproved.ToString()),
            };

            return View(employeeModel);
        }
    }
}
