using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Services.Users;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Services.Supervisors;
using EmployeeHolidayTrackingSystem.Web.Infrastructure;
using EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Employees;

using static EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.SupervisorConstants;
using static EmployeeHolidayTrackingSystem.Web.Areas.Employees.EmployeeConstants;
using EmployeeHolidayTrackingSystem.Web.Models.Users;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Controllers
{
    [Area(SupervisorsAreaName)]
    [Authorize(Roles = SupervisorRoleName)]
    public class EmployeesController : Controller
    {
        private readonly IUserService users;
        private readonly IEmployeeService employees;
        private readonly ISupervisorService supervisors;

        public EmployeesController(IUserService users, IEmployeeService employees, ISupervisorService supervisors)
        {
            this.users = users;
            this.employees = employees;
            this.supervisors = supervisors;
        }

        public IActionResult Add() => View(new UserFormModel());

        [HttpPost]
        public IActionResult Add(UserFormModel model)
        {
            if(this.users.UserWithEmailExists(model.Email!))
            {
                ModelState.AddModelError(nameof(model.Email),
                    "User with this email already exists.");
            }

            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            var supervisorId = this.supervisors.GetSupervisorByUserId(this.User.Id()).Id;

            this.employees.CreateEmployee(model.FirstName!, model.LastName!, 
                model.Email!, model.Password!, supervisorId, EmployeeRoleName);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

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

            if (model.Email != employee.User.Email && this.users.UserWithEmailExists(model.Email!))
            {
                ModelState.AddModelError(nameof(model.Email),
                    "User with this email already exists.");
            }

            if (!this.ModelState.IsValid)
            {
                return View("Details", model);
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

            this.employees.DeleteEmployee(model.Id);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
