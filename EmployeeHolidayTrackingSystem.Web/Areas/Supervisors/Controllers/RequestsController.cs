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

        public IActionResult Respond(Guid id)
        {
            if(!this.requests.RequestExists(id))
            {
                return BadRequest();
            }

            var request = this.requests.GetRequestDetails(id);

            var employeeId = this.requests.GetRequestEmployeeId(request!.Id);

            var model = new PendingRequestDetailsViewModel()
            {
                Id = request!.Id,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Employee = new EmployeeExtendedViewModel()
                {
                    Id = employeeId,
                    FullName = this.employees.GetEmployeeFullName(employeeId),
                    HolidayDaysRemaining = this.employees.GetEmployeeHolidayDaysRemaining(employeeId) ?? 0
                }
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Approve(PendingRequestDetailsViewModel model)
        {
            if (!this.requests.RequestExists(model.Id))
            {
                return BadRequest();
            }

            var startDate = DateTime.Parse(model.StartDate!);
            var endDate = DateTime.Parse(model.EndDate!);

            var holidayDaySpan = endDate.Subtract(startDate);
            var holidayDaysCount = holidayDaySpan.Days + 1;

            var employeeId = this.requests.GetRequestEmployeeId(model.Id);

            // If employee requests more days than they have remaining
            if (!this.employees.CheckIfEmployeeHasEnoughHolidayDays(employeeId, holidayDaysCount))
            {
                TempData["message"] = "The Employee has less holiday days remaining than requested. You cannot approve their request.";
                return View("~/Areas/Supervisors/Views/Requests/Respond.cshtml", model);
            }

            this.requests.UpdateRequestToApproved(model.Id);

            this.employees.SubtractEmployeeHolidayDays(employeeId, holidayDaysCount);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Disapprove(Guid id)
        {
            var model = new DisapprovedRequestFormModel()
            {
                RequestId = id,
                Statement = string.Empty
            }; 

            return View(model);
        }

        [HttpPost]
        public IActionResult Disapprove(DisapprovedRequestFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!this.requests.RequestExists(model.RequestId))
            {
                return BadRequest();
            }

            this.requests.UpdateDisapprovedRequest(model.RequestId, model.Statement);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
