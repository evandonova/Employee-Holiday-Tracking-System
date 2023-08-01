﻿using System.ComponentModel.DataAnnotations;

namespace EmployeeHolidayTrackingSystem.Web.Areas.Supervisors.Models
{
    public class EmployeeExtendedViewModel : EmployeeViewModel
    {
        [Display(Name = "Holiday Days Remaining")]
        public int HolidayDaysRemaining { get; init; }
    }
}
