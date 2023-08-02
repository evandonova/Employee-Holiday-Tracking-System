using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Services.Users;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Services.Supervisors;
using EmployeeHolidayTrackingSystem.Web.Infrastructure;
using EmployeeHolidayTrackingSystem.Web.Areas.Admin.Models.Supervisors;
using EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models.Employees;

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

        public IActionResult Add() => View(new SupervisorFormModel());

        [HttpPost]
        public IActionResult Add(SupervisorFormModel model)
        {
            if (this.users.UserWithEmailExists(model.Email!))
            {
                ModelState.AddModelError(nameof(model.Email),
                    "User with this email already exists.");
            }

            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            this.supervisors.CreateSupervisor(model.FirstName!, model.LastName!,
                model.Email!, model.Password!, SupervisorRoleName);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Details(Guid id)
        {
            var supervisor = this.supervisors.GetSupervisorById(id);

            if (supervisor is null)
            {
                return BadRequest();
            }

            var model = new SupervisorDetailsFormModel()
            {
                Id = supervisor.Id,
                FirstName = supervisor.User.FirstName,
                LastName = supervisor.User.LastName,
                Email = supervisor.User.Email
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(SupervisorDetailsFormModel model)
        {
            var supervisor = this.supervisors.GetSupervisorById(model.Id);

            if (supervisor is null)
            {
                return BadRequest();
            }

            if (model.Email != supervisor.User.Email && this.users.UserWithEmailExists(model.Email!))
            {
                ModelState.AddModelError(nameof(model.Email),
                    "User with this email already exists.");
            }

            if (!this.ModelState.IsValid)
            {
                return View("Details", model);
            }

            this.supervisors.EditSupervisor(model.Id, model.FirstName, model.LastName, model.Email, model.NewPassword);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        public IActionResult Delete(SupervisorDetailsFormModel model)
        {
            var supervisor = this.supervisors.GetSupervisorById(model.Id);

            if (supervisor is null)
            {
                return BadRequest();
            }

            this.supervisors.DeleteSupervisor(model.Id);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
