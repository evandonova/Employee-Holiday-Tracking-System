using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Services.Users;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Services.Supervisors;
using EmployeeHolidayTrackingSystem.Web.Models.Users;
using EmployeeHolidayTrackingSystem.Web.Areas.Admin.Models.Supervisors;

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
        public IActionResult Add(UserFormModel model)
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
            if (!this.supervisors.SupervisorExists(id))
            {
                return BadRequest();
            }

            var supervisor = this.supervisors.GetDetails(id);

            var model = new SupervisorDetailsFormModel()
            {
                Id = supervisor!.Id,
                FirstName = supervisor.FirstName,
                LastName = supervisor.LastName,
                Email = supervisor.Email
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(SupervisorDetailsFormModel model)
        {
            if (!this.supervisors.SupervisorExists(model.Id))
            {
                return BadRequest();
            }

            if (model.Email != this.supervisors.GetSupervisorEmail(model.Id) && this.users.UserWithEmailExists(model.Email!))
            {
                ModelState.AddModelError(nameof(model.Email),
                    "User with this email already exists.");
            }

            if (!this.ModelState.IsValid)
            {
                return View("Details", model);
            }

            this.supervisors.EditSupervisor(model.Id, model.FirstName!, model.LastName!, model.Email!, model.NewPassword);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        public IActionResult Delete(SupervisorDetailsFormModel model)
        {
            if (!this.supervisors.SupervisorExists(model.Id))
            {
                return BadRequest();
            }

            this.supervisors.DeleteSupervisor(model.Id);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
