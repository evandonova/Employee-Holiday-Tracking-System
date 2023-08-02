using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Web.Infrastructure;
using EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models;
using EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models.Requests;
using EmployeeHolidayTrackingSystem.Services.Requests;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Services.RequestStatuses;

using static EmployeeHolidayTrackingSystem.Web.Constants;
using static EmployeeHolidayTrackingSystem.Web.Areas.Employees.EmployeeConstants;

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
            var request = this.requests.GetRequestById(id);

            if (request is null)
            {
                return BadRequest();
            }

            var requestStatusTitle = this.statuses.GetStatusTitleById(request.StatusId);

            var model = new EmployeeRequestViewModel()
            {
                StartDate = request.StartDate.ToString(DateFormat),
                EndDate = request.EndDate.ToString(DateFormat),
                Status = requestStatusTitle ?? "Pending",
                DisapprovalStatement = request.DisapprovalStatement
            };

            return View(model);
        }

        public IActionResult Create()
        {
            var dateToday = DateTime.UtcNow;

            var model = new RequestFormModel()
            {
                StartDate = dateToday.ToString(DateFormat),
                EndDate = dateToday.ToString(DateFormat)
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

            var currentEmployee = employees.GetEmployeeByUserId(this.User.Id());

            var holidayDaySpan = endDate.Subtract(startDate);
            var holidayDaysCount = holidayDaySpan.Days + 1;

            // If employee requests more days than they have remaining
            if (holidayDaysCount > currentEmployee.HolidayDaysRemaining)
            {
                TempData["message"] = "You try to request more holiday days than you have remaining.";
                return View(requestModel);
            }

            this.requests.Create(startDate, endDate, currentEmployee.Id, currentEmployee.SupervisorId);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
