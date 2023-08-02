using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Employees;

using static EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.SupervisorConstants;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Controllers
{
    [Area(SupervisorsAreaName)]
    [Authorize(Roles = SupervisorRoleName)]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService employees;

        public EmployeesController(IEmployeeService employees)
            => this.employees = employees;

        public IActionResult Details(Guid id)
        {
            var employee = this.employees.GetEmployeeById(id);

            if (employee is null)
            {
                return BadRequest();
            }

            var model = new EmployeeDetailsFormModel()
            {
                Id = employee.Id,
                FirstName = employee.User.FirstName,
                LastName = employee.User.LastName,
                HolidayDaysRemaining = employee.HolidayDaysRemaining,
                Email = employee.User.Email
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeDetailsFormModel model)
        {
            var employee = this.employees.GetEmployeeById(model.Id);

            if (employee is null)
            {
                return BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View("Details", model);
            }

            this.employees.EditEmployee(model.Id, model.FirstName, model.LastName, model.Email, model.NewPassword);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        public IActionResult Delete(EmployeeDetailsFormModel model)
        {
            var employee = this.employees.GetEmployeeById(model.Id);

            if (employee is null)
            {
                return BadRequest();
            }
            //TODO

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
