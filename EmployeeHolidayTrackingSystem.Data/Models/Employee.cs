﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EmployeeHolidayTrackingSystem.Data.Models
{
    using static DataConstants.Employee;

    public class Employee
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public int HolidayDaysRemaining { get; set; } = InitialHolidayDaysCount;

        [Required]
        public string UserId { get; init; } = null!;

        public IdentityUser User { get; init; } = null!;

        public Guid SupervisorId { get; init; }

        public Supervisor Supervisor { get; init; } = null!;

        public IEnumerable<HolidayRequest> HolidayRequests { get; set; } = new List<HolidayRequest>();
    }
}
