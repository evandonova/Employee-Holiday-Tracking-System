using Microsoft.EntityFrameworkCore;
using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;

namespace EmployeeHolidayTrackingSystem.Services.Supervisors
{
    public class SupervisorService : ISupervisorService
    {

        private readonly EmployeeHolidayDbContext data;

        public SupervisorService(EmployeeHolidayDbContext data)
            => this.data = data;

        public Supervisor GetSupervisor(string? userId)
            => this.data.Supervisors
                    .Include(s => s.User)
                    .Include(s => s.Employees)
                    .Include(s => s.HolidayRequests)
                    .First(s => s.UserId == userId);
    }
}
