using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Web.Infrastructure;
using EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models;
using EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models.Requests;
using EmployeeHolidayTrackingSystem.Services.Requests;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Services.RequestStatuses;

using static EmployeeHolidayTrackingSystem.Data.DataConstants.HolidayRequest;
using static EmployeeHolidayTrackingSystem.Web.Areas.Employees.EmployeeConstants;
using System.Globalization;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Employees.Controllers
{
    [Area(EmployeesAreaName)]
    [Authorize(Roles = EmployeeRoleName)]
    public class RequestsController : Controller
    {
        private readonly IRequestService requests;
        private readonly IEmployeeService employees;
        private readonly IRequestStatusService statuses;

        public RequestsController(IRequestService requests, 
            IEmployeeService employees, IRequestStatusService statuses)
        {
            this.requests = requests;
            this.employees = employees;
            this.statuses = statuses;
        }

        public IActionResult Details(Guid id)
        {
            if (!this.requests.RequestExists(id))
            {
                return BadRequest();
            }

            var request = this.requests.GetRequestDetails(id);

            var model = new EmployeeRequestViewModel()
            {
                Id = request!.Id,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = request.Status,
                DisapprovalStatement = request.DisapprovalStatement
            };

            return View(model);
        }

        public IActionResult Create()
        {
            var dateToday = DateTime.UtcNow;

            var model = new RequestFormModel()
            {
                StartDate = dateToday.ToString(DateFormat, CultureInfo.InvariantCulture),
                EndDate = dateToday.ToString(DateFormat, CultureInfo.InvariantCulture)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(RequestFormModel requestModel)
        {
            var startDate = DateTime.Parse(requestModel.StartDate);
            var endDate = DateTime.Parse(requestModel.EndDate);

            // If end date is before start date, add error message
            if (endDate < startDate)
            {
                ModelState.AddModelError(nameof(requestModel.EndDate),
                    "End date should be after start date.");
            }

            if (!ModelState.IsValid)
            {
                return View(requestModel);
            }

            var holidayDaySpan = endDate.Subtract(startDate);
            var holidayDaysCount = holidayDaySpan.Days + 1;

            var employeeId = this.employees.GetEmployeeIdByUserId(this.User.Id()!);

            // If employee requests more days than they have remaining
            if (!this.employees.CheckIfEmployeeHasEnoughHolidayDays(employeeId, holidayDaysCount))
            {
                TempData["message"] = "You try to request more holiday days than you have remaining.";
                return View(requestModel);
            }

            var supervisorId = this.employees.GetEmployeeSupervisorId(employeeId);

            this.requests.Create(startDate, endDate, employeeId, supervisorId);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
