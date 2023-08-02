using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Services.Users;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Services.Supervisors;
using EmployeeHolidayTrackingSystem.Web.Models.Users;
using EmployeeHolidayTrackingSystem.Web.Infrastructure;
using EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Employees;

using static EmployeeHolidayTrackingSystem.Web.Areas.Employees.EmployeeConstants;
using static EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.SupervisorConstants;

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

            var supervisorId = this.supervisors.GetSupervisorIdByUserId(this.User.Id()!);

            this.employees.CreateEmployee(model.FirstName!, model.LastName!, 
                model.Email!, model.Password!, supervisorId, EmployeeRoleName);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Details(Guid id)
        {
            if (!this.employees.EmployeeExists(id))
            {
                return BadRequest();
            }

            var employeeData = this.employees.GetEmployeeDetails(id);

            var model = new EmployeeDetailsFormModel()
            {
                Id = employeeData.Id,
                FirstName = employeeData.FirstName,
                LastName = employeeData.LastName,
                HolidayDaysRemaining = employeeData.HolidayDaysRemaining,
                Email = employeeData.Email
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeDetailsFormModel model)
        {
            if (!this.employees.EmployeeExists(model.Id))
            {
                return BadRequest();
            }

            if (model.Email != this.employees.GetEmployeeEmail(model.Id) && this.users.UserWithEmailExists(model.Email!))
            {
                ModelState.AddModelError(nameof(model.Email),
                    "User with this email already exists.");
            }

            if (!this.ModelState.IsValid)
            {
                return View("Details", model);
            }

            this.employees.EditEmployee(model.Id, model.FirstName!, model.LastName!, model.Email!, model.NewPassword);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        public IActionResult Delete(EmployeeDetailsFormModel model)
        {
            if (!this.employees.EmployeeExists(model.Id))
            {
                return BadRequest();
            }

            this.employees.DeleteEmployee(model.Id);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
