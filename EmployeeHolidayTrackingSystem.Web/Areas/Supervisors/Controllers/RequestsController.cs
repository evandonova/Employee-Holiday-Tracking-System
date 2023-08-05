using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Services.Requests;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Services.RequestStatuses;
using EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Requests;
using EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Employees;

using static EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.SupervisorConstants;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Controllers
{
    [Area(SupervisorsAreaName)]
    [Authorize(Roles = SupervisorRoleName)]
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

        public async Task<IActionResult> Respond(string id)
        {
            if(!await this.requests.RequestExistsAsync(id))
            {
                return BadRequest();
            }

            var request = await this.requests.GetRequestDetailsAsync(id);

            var employeeId = await this.requests.GetRequestEmployeeIdAsync(request.Id);

            var model = new PendingRequestDetailsViewModel()
            {
                Id = request!.Id,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Employee = new EmployeeExtendedViewModel()
                {
                    Id = employeeId,
                    FullName = await this.employees.GetEmployeeFullNameAsync(employeeId),
                    HolidayDaysRemaining = await this.employees.GetEmployeeHolidayDaysRemainingAsync(employeeId)
                }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(PendingRequestDetailsViewModel model)
        {
            if (!await this.requests.RequestExistsAsync(model.Id))
            {
                return BadRequest();
            }

            var startDate = DateTime.Parse(model.StartDate);
            var endDate = DateTime.Parse(model.EndDate);

            var holidayDaySpan = endDate.Subtract(startDate);
            var holidayDaysCount = holidayDaySpan.Days + 1;

            var employeeId = await this.requests.GetRequestEmployeeIdAsync(model.Id);

            // If employee requests more days than they have remaining
            if (!await this.employees.CheckIfEmployeeHasEnoughHolidayDaysAsync(employeeId, holidayDaysCount))
            {
                TempData["message"] = "The Employee has less holiday days remaining than requested. You cannot approve their request.";
                return View("~/Areas/Supervisors/Views/Requests/Respond.cshtml", model);
            }

            await this.requests.UpdateRequestToApprovedAsync(model.Id);

            await this.employees.SubtractEmployeeHolidayDaysAsync(employeeId, holidayDaysCount);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Disapprove() => View();

        [HttpPost]
        public async Task<IActionResult> Disapprove(string id, DisapprovedRequestFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!await this.requests.RequestExistsAsync(id))
            {
                return BadRequest();
            }

            await this.requests.UpdateDisapprovedRequestAsync(id, model.Statement);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
