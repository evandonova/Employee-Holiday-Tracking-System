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

            try
            {
                await this.supervisors.CreateSupervisorAsync(model.FirstName, model.LastName,
                    model.Email, model.Password, SupervisorRoleName);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Unexpected error occurred while trying to add your supervisor! Please try again later or contact administrator!";
                return View(model);
            }

            TempData["SuccessMessage"] = "You have successfully added a new supervisor.";

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

            try
            {
                await this.supervisors.EditSupervisorAsync(model.Id, model.FirstName, model.LastName, 
                    model.Email, model.NewPassword?.Trim());
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Unexpected error occurred while trying to edit your supervisor! Please try again later or contact administrator!";
                return View("Details", model);
            }

            TempData["SuccessMessage"] = "You have successfully edited the supervisor.";

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserDetailsBaseFormModel model)
        {
            if (!await this.supervisors.SupervisorExistsAsync(model.Id))
            {
                return BadRequest();
            }

            try
            {
                await this.supervisors.DeleteSupervisorAsync(model.Id);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Unexpected error occurred while trying to delete your supervisor! Please try again later or contact administrator!";
                return View("Details", model);
            }

            TempData["SuccessMessage"] = "You have successfully deleted the supervisor.";
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
