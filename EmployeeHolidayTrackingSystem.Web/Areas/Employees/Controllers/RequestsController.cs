using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Web.Areas.Shared;
using EmployeeHolidayTrackingSystem.Web.Infrastructure;
using EmployeeHolidayTrackingSystem.Web.Areas.Employees.Models.Requests;
using EmployeeHolidayTrackingSystem.Services.Requests;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Services.RequestStatuses;

using static EmployeeHolidayTrackingSystem.Data.DataConstants.HolidayRequest;
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

        public async Task<IActionResult> Details(string id)
        {
            if (!await this.requests.RequestExistsAsync(id))
            {
                return BadRequest();
            }

            var request = await this.requests.GetRequestDetailsAsync(id);

            var model = new EmployeeRequestViewModel()
            {
                Id = request.Id,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = Task
                            .Run(async () =>
                            {
                                return await this.requests
                                    .GetRequestStatusTitleAsync(request.Id);
                            })
                           .GetAwaiter()
                           .GetResult(),
                DisapprovalStatement = Task
                                        .Run(async () =>
                                        {
                                            return await this.requests
                                                .GetDisapprovalStatementAsync(request.Id);
                                        })
                                       .GetAwaiter()
                                       .GetResult()
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
        public async Task<IActionResult> Create(RequestFormModel requestModel)
        {
            var startDate = DateTime.Parse(requestModel.StartDate);
            var endDate = DateTime.Parse(requestModel.EndDate);

            // If the end date is before the start date, show error message
            if (endDate < startDate)
            {
                ModelState.AddModelError(nameof(requestModel.EndDate),
                    "The end date should be after the start date.");
            }

            if (startDate.DayOfWeek == DayOfWeek.Saturday 
                || startDate.DayOfWeek == DayOfWeek.Sunday)
            {
                ModelState.AddModelError(nameof(requestModel.StartDate),
                    "Start date cannot be a weekend day.");
            }

            if (endDate.DayOfWeek == DayOfWeek.Saturday
                || endDate.DayOfWeek == DayOfWeek.Sunday)
            {
                ModelState.AddModelError(nameof(requestModel.EndDate),
                    "End date cannot be a weekend day.");
            }

            if (!ModelState.IsValid)
            {
                return View(requestModel);
            }

            var employeeId = await this.employees.GetEmployeeIdByUserIdAsync(this.User.Id()!);
            var holidayDaysCount = BusinessDaysCounter.GetDaysCount(startDate, endDate);

            // If employee requests more days than they have remaining
            if (!await this.employees.CheckIfEmployeeHasEnoughHolidayDaysAsync(employeeId, holidayDaysCount))
            {
                TempData["ViewMessage"] = "You try to request more holiday days than you have remaining.";
                return View(requestModel);
            }

            var supervisorId = await this.employees.GetEmployeeSupervisorIdAsync(employeeId);

            try
            {
                await this.requests.CreateAsync(startDate, endDate, employeeId, supervisorId);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Unexpected error occurred while trying to create your request! Please try again later or contact administrator!";
                return View(requestModel);
            }

            TempData["SuccessMessage"] = "You have successfully created a new holiday request.";

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
