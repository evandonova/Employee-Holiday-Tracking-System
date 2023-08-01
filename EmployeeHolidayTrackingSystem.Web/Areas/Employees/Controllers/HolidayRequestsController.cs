using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;
using EmployeeHolidayTrackingSystem.Web.Controllers;
using EmployeeHolidayTrackingSystem.Web.Infrastructure;
using EmployeeHolidayTrackingSystem.Web.Models.HolidayRequests;

using static EmployeeHolidayTrackingSystem.Web.Areas.Employees.EmployeeConstants;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Employees.Controllers
{
    [Area(EmployeesAreaName)]
    [Authorize(Roles = EmployeeRoleName)]
    public class HolidayRequestsController : Controller
    {
        private readonly EmployeeHolidayDbContext data;

        public HolidayRequestsController(EmployeeHolidayDbContext data)
            => this.data = data;

        public IActionResult Details(Guid id)
        {
            var request = this.data.HolidayRequests.Find(id);

            if (request is null)
            {
                return BadRequest();
            }

            var requestStatusTitle = this.data.HolidayRequestStatuses?
                .Find(request.StatusId)?
                .Title;

            var model = new HolidayRequestViewModel()
            {
                StartDate = request.StartDate.ToString("dd MMMM yyyy"),
                EndDate = request.EndDate.ToString("dd MMMM yyyy"),
                Status = requestStatusTitle ?? "Pending" 
            };

            return View(model);
        }

        public IActionResult Create()
        {
            var dateToday = DateTime.UtcNow;

            var model = new HolidayRequestFormModel()
            {
                StartDate = dateToday.ToString("dd MMMM yyyy"),
                EndDate = dateToday.ToString("dd MMMM yyyy")
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(HolidayRequestFormModel requestModel)
        {
            var startDate = DateTime.Parse(requestModel.StartDate);
            var endDate = DateTime.Parse(requestModel.EndDate);

            // If end date is before start date, add error message
            if (endDate < startDate)
            {
                ModelState.AddModelError(nameof(requestModel.EndDate),
                    "End date should be after start date.");
            }

            var currentEmployee = GetEmployee(User.Id());

            var holidayDaySpan = endDate.Subtract(startDate);
            var holidayDaysCount = holidayDaySpan.Days + 1;

            // If employee requests more days than they have remaining
            if (holidayDaysCount > currentEmployee.HolidayDaysRemaining)
            {
                ModelState.AddModelError(nameof(requestModel.EndDate),
                    "You don't have enough holiday days remaining.");
            }

            if (!ModelState.IsValid)
            {
                return View(requestModel);
            }

            var request = new HolidayRequest()
            {
                StartDate = startDate,
                EndDate = endDate,
                StatusId = GetPendingStatusId(),
                EmployeeId = currentEmployee.Id,
                SupervisorId = currentEmployee.SupervisorId,
            };

            data.HolidayRequests.Add(request);
            data.SaveChanges();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private Employee GetEmployee(string? userId)
            => data.Employees
                    .First(e => e.UserId == userId);

        private int GetPendingStatusId()
            => data.HolidayRequestStatuses
                    .First(s => s.Title == HolidayRequestStatusEnum.Pending.ToString())
                    .Id;
    }
}
