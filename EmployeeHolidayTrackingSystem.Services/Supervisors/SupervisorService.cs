using Microsoft.EntityFrameworkCore;
using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;
using EmployeeHolidayTrackingSystem.Services.Users;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Services.Supervisors.Models;

namespace EmployeeHolidayTrackingSystem.Services.Supervisors
{
    public class SupervisorService : ISupervisorService
    {
        private readonly EmployeeHolidayDbContext data;
        private readonly IUserService users;
        private readonly IEmployeeService employees;

        public SupervisorService(EmployeeHolidayDbContext data,
            IUserService users, IEmployeeService employees)
        {
            this.data = data;
            this.users = users;
            this.employees = employees;
        }

        public async Task<string> GetSupervisorIdByUserIdAsync(string userId)
        {
            var supervisor = await this.data.Supervisors.FirstAsync(s => s.UserId == userId);
            return supervisor.Id.ToString();
        }

        public async Task<string> GetSupervisorFullNameAsync(string supervisorId)
            => await this.data.Supervisors
                .Where(s => s.Id.ToString() == supervisorId)
                .Select(s => $"{s.User.FirstName} {s.User.LastName}")
                .FirstAsync();

        public async Task<bool> SupervisorExistsAsync(string supervisorId)
            => await this.data.Supervisors.AnyAsync(s => s.Id.ToString() == supervisorId);

        public async Task<string> GetSupervisorEmailAsync(string supervisorId)
           => await this.data.Supervisors
              .Where(s => s.Id.ToString() == supervisorId)
              .Select(s => s.User.Email)
              .FirstAsync();

        public async Task<SupervisorDetailsServiceModel> GetSupervisorDetailsAsync(string supervisorId)
            => await this.data.Supervisors
                .Where(s => s.Id.ToString() == supervisorId)
                .Select(s => new SupervisorDetailsServiceModel()
                {
                    Id = s.Id.ToString(),
                    FirstName = s.User.FirstName,
                    LastName = s.User.LastName,
                    Email = s.User.Email
                })
                .FirstAsync();

        public async Task<List<SupervisorServiceModel>> GetAllSupervisorsAsync()
            => await this.data.Supervisors
                .Select(s => new SupervisorServiceModel()
                {
                    Id = s.Id.ToString(),
                    FullName = $"{s.User.FirstName} {s.User.LastName}"
                })
                .ToListAsync();

        public async Task CreateSupervisorAsync(string firstName, string lastName,
            string email, string password, string supervisorRoleName)
        {
            var newUserId = await this.users.CreateUserAndReturnIdAsync(firstName, lastName, email, password);

            await this.users.AddUserToRoleAsync(newUserId, supervisorRoleName);

            var newSupervisor = new Supervisor()
            {
                UserId = newUserId
            };

            await this.data.Supervisors.AddAsync(newSupervisor);
            await this.data.SaveChangesAsync();
        }

        public async Task EditSupervisorAsync(string id, string firstName, string lastName,
            string email, string? newPassword)
        {
            var supervisor = await this.data.Supervisors
                .Include(s => s.User)
                .FirstAsync(s => s.Id.ToString() == id);

            supervisor.User.FirstName = firstName;
            supervisor.User.LastName = lastName;

            await this.users.UpdateEmailAsync(supervisor.UserId, email);

            if(newPassword is not null)
            {
                await this.users.UpdatePasswordAsync(supervisor.UserId, newPassword);
            }

            await this.data.SaveChangesAsync();
        }

        public async Task DeleteSupervisorAsync(string supervisorId)
        {
            var supervisor = await this.data.Supervisors.FirstAsync(s => s.Id.ToString() == supervisorId);

            await this.employees.DeleteEmployeesBySupervisorIdAsync(supervisor.Id.ToString());

            this.data.Supervisors.Remove(supervisor);

            await this.users.DeleteUserAsync(supervisor.UserId);

            await this.data.SaveChangesAsync();
        }
    }
}
