using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Services.Users;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Services.Supervisors;
using EmployeeHolidayTrackingSystem.Web.Infrastructure;
using EmployeeHolidayTrackingSystem.Web.Areas.Shared.Models;
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

        public EmployeesController(IUserService users, IEmployeeService employees,
            ISupervisorService supervisors)
        {
            this.users = users;
            this.employees = employees;
            this.supervisors = supervisors;
        }

        public IActionResult Add() => View(new UserFormModel());

        [HttpPost]
        public async Task<IActionResult> Add(UserFormModel model)
        {
            if (await this.users.UserWithEmailExistsAsync(model.Email))
            {
                ModelState.AddModelError(nameof(model.Email),
                    "User with this email already exists.");
            }

            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            var supervisorId = await this.supervisors.GetSupervisorIdByUserIdAsync(this.User.Id()!);

            try
            {
                await this.employees.CreateEmployeeAsync(model.FirstName, model.LastName,
                    model.Email, model.Password, supervisorId, EmployeeRoleName);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Unexpected error occurred while trying to add your new employee! Please try again later or contact administrator!";
                return View(model);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> Details(string id)
        {
            if (!await this.employees.EmployeeExistsAsync(id))
            {
                return BadRequest();
            }

            var employeeData = await this.employees.GetEmployeeDetailsAsync(id);

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
        public async Task<IActionResult> Edit(EmployeeDetailsFormModel model)
        {
            if (!await this.employees.EmployeeExistsAsync(model.Id))
            {
                return BadRequest();
            }

            if (model.Email != await this.employees.GetEmployeeEmailAsync(model.Id)
                && await this.users.UserWithEmailExistsAsync(model.Email))
            {
                ModelState.AddModelError(nameof(model.Email),
                    "User with this email already exists.");
            }

            if (!this.ModelState.IsValid)
            {
                return View("Details", model);
            }

            try
            {
                await this.employees.EditEmployeeAsync(model.Id, model.FirstName,
                    model.LastName, model.Email, model.NewPassword?.Trim());
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Unexpected error occurred while trying to edit your employee! Please try again later or contact administrator!";
                return View("Details", model);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeDetailsFormModel model)
        {
            if (!await this.employees.EmployeeExistsAsync(model.Id))
            {
                return BadRequest();
            }

            try
            {
                await this.employees.DeleteEmployeeAsync(model.Id);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Unexpected error occurred while trying to delete your employee! Please try again later or contact administrator!";
                return View("Details", model);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
