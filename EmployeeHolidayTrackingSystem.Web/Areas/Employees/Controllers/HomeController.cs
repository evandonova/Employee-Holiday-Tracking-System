using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;
using EmployeeHolidayTrackingSystem.Web.Infrastructure;
using EmployeeHolidayTrackingSystem.Web.Models.HolidayRequests;
using EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models;

using static EmployeeHolidayTrackingSystem.Web.Areas.Employees.EmployeeConstants;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Employees.Controllers
{
    [Area(EmployeesAreaName)]
    [Authorize(Roles = EmployeeRoleName)]
    public class HomeController : Controller
    {
        private readonly EmployeeHolidayDbContext data;

        public HomeController(EmployeeHolidayDbContext data)
            => this.data = data;

        public IActionResult Index()
        {
            var employee = GetEmployee(this.User.Id());

            var holidayRequests = employee.HolidayRequests
                    .Select(hr => new HolidayRequestViewModel
                    {
                        Id = hr.Id,
                        StartDate = hr.StartDate.ToString("dd MMMM yyyy"),
                        EndDate = hr.EndDate.ToString("dd MMMM yyyy"),
                        Status = this.data.HolidayRequestStatuses.Find(hr.StatusId).Title
                    })
                    .ToList();

            var supervisor = employee.Supervisor;

            var employeeModel = new EmployeeViewModel()
            {
                FullName = $"{employee.User.FirstName} {employee.User.LastName}",
                SupervisorName = $"{supervisor.User.FirstName} {supervisor.User.LastName}",
                HolidayDaysRemaining = employee.HolidayDaysRemaining,
                PendingHolidayRequests = holidayRequests
                    .Where(h => h.Status == HolidayRequestStatusEnum.Pending.ToString()),
                ApprovedHolidayRequests = holidayRequests
                    .Where(h => h.Status == HolidayRequestStatusEnum.Approved.ToString()),
                DisapprovedHolidayRequests = holidayRequests
                    .Where(h => h.Status == HolidayRequestStatusEnum.Disapproved.ToString()),
            };

            return View(employeeModel);
        }

        private Employee GetEmployee(string? userId)
            => data.Employees
                    .Include(e => e.User)
                    .Include(e => e.Supervisor.User)
                    .Include(e => e.HolidayRequests)
                    .First(e => e.UserId == userId);
    }
}
