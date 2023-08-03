using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeHolidayTrackingSystem.Services.Users;
using EmployeeHolidayTrackingSystem.Services.Supervisors;
using EmployeeHolidayTrackingSystem.Web.Infrastructure;
using EmployeeHolidayTrackingSystem.Web.Areas.Admin.Models;
using EmployeeHolidayTrackingSystem.Web.Areas.Admin.Models.Supervisors;

using static EmployeeHolidayTrackingSystem.Web.Areas.Admin.AdminConstants;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Admin.Controllers
{
    [Area(AdminAreaName)]
    [Authorize(Roles = AdminRoleName)]
    public class HomeController : Controller
    {
        private readonly ISupervisorService supervisors;
        private readonly IUserService users;

        public HomeController(ISupervisorService supervisors, IUserService users)
        {
            this.supervisors = supervisors;
            this.users = users;
        }

        public async Task<IActionResult> Index()
        {
            var supervisors = await this.supervisors.GetAllSupervisorsAsync();

            var model = new AdminProfileViewModel()
            {
                FullName = await this.users.GetUserFullNameAsync(this.User.Id()!),
                Supervisors = supervisors.Select(s => new SupervisorViewModel
                {
                    Id = s.Id,
                    FullName = s.FullName
                })
            };

            return View(model);
        }
    }
}
