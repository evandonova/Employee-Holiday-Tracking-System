using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Services.Users;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Services.Supervisors;
using EmployeeHolidayTrackingSystem.Web.Areas.Shared.Models;

using static EmployeeHolidayTrackingSystem.Web.Areas.Admin.AdminConstants;
using static EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.SupervisorConstants;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Admin.Controllers
{
    [Area(AdminAreaName)]
    [Authorize(Roles = AdminRoleName)]
    public class SupervisorsController : Controller
    {
        private readonly IUserService users;
        private readonly IEmployeeService employees;
        private readonly ISupervisorService supervisors;

        public SupervisorsController(IUserService users, IEmployeeService employees, ISupervisorService supervisors)
        {
            this.users = users;
            this.employees = employees;
            this.supervisors = supervisors;
        }

        public IActionResult Add() => View(new UserFormModel());

        [HttpPost]
        public async Task<IActionResult> Add(UserFormModel model)
        {
            if (await this.users.UserWithEmailExistsAsync(model.Email!))
            {
                ModelState.AddModelError(nameof(model.Email),
                    "User with this email already exists.");
            }

            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            await this.supervisors.CreateSupervisorAsync(model.FirstName, model.LastName,
                model.Email, model.Password, SupervisorRoleName);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> Details(string id)
        {
            if (!await this.supervisors.SupervisorExistsAsync(id))
            {
                return BadRequest();
            }

            var supervisor = await this.supervisors.GetSupervisorDetailsAsync(id);

            var model = new UserDetailsBaseFormModel()
            {
                Id = supervisor.Id,
                FirstName = supervisor.FirstName,
                LastName = supervisor.LastName,
                Email = supervisor.Email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserDetailsBaseFormModel model)
        {
            if (!await this.supervisors.SupervisorExistsAsync(model.Id))
            {
                return BadRequest();
            }

            if (model.Email != await this.supervisors.GetSupervisorEmailAsync(model.Id) 
                && await this.users.UserWithEmailExistsAsync(model.Email))
            {
                ModelState.AddModelError(nameof(model.Email),
                    "User with this email already exists.");
            }

            if (!this.ModelState.IsValid)
            {
                return View("Details", model);
            }

            await this.supervisors.EditSupervisorAsync(model.Id, model.FirstName, model.LastName, model.Email, model.NewPassword?.Trim());

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!await this.supervisors.SupervisorExistsAsync(id))
            {
                return BadRequest();
            }

            await this.supervisors.DeleteSupervisorAsync(id);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
