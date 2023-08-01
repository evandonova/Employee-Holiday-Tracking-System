using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Web.Infrastructure;
using EmployeeHolidayTrackingSystem.Web.Models.Requests;
using EmployeeHolidayTrackingSystem.Services.Requests;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Services.RequestStatuses;

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
            var request = this.requests.GetById(id);

            if (request is null)
            {
                return BadRequest();
            }

            var requestStatusTitle = this.statuses.GetTitleById(request.StatusId);

            var model = new RequestViewModel()
            {
                StartDate = request.StartDate.ToString("d MMMM yyyy"),
                EndDate = request.EndDate.ToString("d MMMM yyyy"),
                Status = requestStatusTitle ?? "Pending" 
            };

            return View(model);
        }

        public IActionResult Create()
        {
            var dateToday = DateTime.UtcNow;

            var model = new RequestFormModel()
            {
                StartDate = dateToday.ToString("d MMMM yyyy"),
                EndDate = dateToday.ToString("d MMMM yyyy")
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
