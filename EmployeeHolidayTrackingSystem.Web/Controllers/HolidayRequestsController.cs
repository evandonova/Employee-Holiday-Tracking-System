using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;
using EmployeeHolidayTrackingSystem.Web.Models.HolidayRequests;
using EmployeeHolidayTrackingSystem.Web.Infrastructure;

namespace EmployeeHolidayTrackingSystem.Web.Controllers
{
    [Authorize] // Think how not to be accessible for supervisors and admin
    public class HolidayRequestsController : Controller
    {
        private readonly EmployeeHolidayDbContext data;

        public HolidayRequestsController(EmployeeHolidayDbContext data) 
            => this.data = data;

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
            if(endDate < startDate)
            {
                ModelState.AddModelError(nameof(requestModel.EndDate),
                    "End date should be after start date.");
            }

            var currentEmployee = GetEmployee(this.User.Id());

            var holidayDaySpan = endDate.Subtract(startDate);
            var holidayDaysCount = holidayDaySpan.Days + 1;

            if(holidayDaysCount > currentEmployee.HolidayDaysRemaining)
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

            this.data.HolidayRequests.Add(request);

            currentEmployee.HolidayDaysRemaining -= holidayDaysCount;

            this.data.SaveChanges();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private Employee GetEmployee(string? userId)
            => this.data.Employees
                    .First(e => e.UserId == userId);

        private int GetPendingStatusId()
            => this.data.HolidayRequestStatuses
                    .First(s => s.Title == HolidayRequestStatusEnum.Pending.ToString())
                    .Id;
    }
}
