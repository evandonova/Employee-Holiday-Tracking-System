using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Services.Requests;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Services.RequestStatuses;
using EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Requests;
using EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Employees;

using static EmployeeHolidayTrackingSystem.Web.Constants;
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
            var request = this.requests.GetRequestById(id);

            if(request == null)
            {
                return BadRequest();
            }

            var model = new PendingRequestDetailsViewModel()
            {
                Id = request.Id,
                StartDate = request.StartDate.ToString(DateFormat),
                EndDate = request.EndDate.ToString(DateFormat),
                Employee = new EmployeeExtendedViewModel()
                {
                    Id = request.EmployeeId,
                    FullName = this.employees.GetEmployeeFullName(request.EmployeeId),
                    HolidayDaysRemaining = this.employees.GetEmployeeHolidayDaysRemaining(request.EmployeeId) ?? 0
                }
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Respond(PendingRequestDetailsViewModel model, bool IsApproved)
        {
            var request = this.requests.GetRequestById(model.Id);

            if (request == null)
            {
                return BadRequest();
            }

            if (!IsApproved) 
            { 
                return RedirectToAction(nameof(RequestsController.Disapprove), "Requests", new { id = model.Id });
            }

            var startDate = DateTime.Parse(model.StartDate);
            var endDate = DateTime.Parse(model.EndDate);

            var holidayDaySpan = endDate.Subtract(startDate);
            var holidayDaysCount = holidayDaySpan.Days + 1;

            // If employee requests more days than they have remaining
            if (!this.employees.CheckIfEmployeeHasEnoughHolidayDays(request.EmployeeId, holidayDaysCount))
            {
                TempData["message"] = "The Employee has less holiday days remaining than requested. You cannot approve their request.";
                return View(model);
            }

            this.requests.UpdateRequestToApproved(request.Id);

            this.employees.SubtractEmployeeHolidayDays(request.EmployeeId, holidayDaysCount);

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

            var request = this.requests.GetRequestById(model.RequestId);

            if (request == null)
            {
                return BadRequest();
            }

            this.requests.UpdateDisapprovedRequest(model.RequestId, model.Statement);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
