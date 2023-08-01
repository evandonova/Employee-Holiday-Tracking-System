﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeHolidayTrackingSystem.Web.Models;

using static EmployeeHolidayTrackingSystem.Web.Areas.Employees.EmployeeConstants;
using static EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.SupervisorConstants;

namespace EmployeeHolidayTrackingSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (this.User.IsInRole(EmployeeRoleName))
            {
                return RedirectToAction("Index", "Home", new { area = EmployeesAreaName });
            }

            if (this.User.IsInRole(SupervisorRoleName))
            {
                return RedirectToAction("Index", "Home", new { area = SupervisorsAreaName });
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}