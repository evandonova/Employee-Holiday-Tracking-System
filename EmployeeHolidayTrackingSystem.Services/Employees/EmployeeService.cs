using Microsoft.EntityFrameworkCore;
using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;
using EmployeeHolidayTrackingSystem.Services.Users;
using EmployeeHolidayTrackingSystem.Services.Requests;
using EmployeeHolidayTrackingSystem.Services.Employees.Models;

using static EmployeeHolidayTrackingSystem.Data.DataConstants.Employee;

namespace EmployeeHolidayTrackingSystem.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeHolidayDbContext data;
        private readonly IUserService users;
        private readonly IRequestService requests;

        public EmployeeService(EmployeeHolidayDbContext data,
            IUserService users, IRequestService requests)
        {
            this.data = data;
            this.users = users;
            this.requests = requests;
        }

        public async Task<string> GetEmployeeIdByUserIdAsync(string userId)
        {
            var employee = await this.data.Employees.FirstAsync(e => e.UserId == userId);
            return employee.Id.ToString();
        }

        public async Task<bool> EmployeeExistsAsync(string employeeId)
            => await this.data.Employees.AnyAsync(e => e.Id.ToString() == employeeId);

        public async Task<string> GetEmployeeEmailAsync(string employeeId)
            => await this.data.Employees
               .Where(e => e.Id.ToString() == employeeId)
               .Select(e => e.User.Email)
               .FirstAsync();

        public async Task<string> GetEmployeeFullNameAsync(string employeeId)
            => await this.data.Employees.Include(e => e.User)
                .Where(e => e.Id.ToString() == employeeId)
                .Select(e => $"{e.User.FirstName} {e.User.LastName}")
                .FirstAsync();

        public async Task<string> GetEmployeeSupervisorIdAsync(string employeeId)
        {
            var employee = await this.data.Employees.FirstAsync(e => e.Id.ToString() == employeeId);
            return employee.SupervisorId.ToString();
        }

        public async Task<int> GetEmployeeHolidayDaysRemainingAsync(string employeeId)
        {
            var employee = await this.data.Employees.FirstAsync(e => e.Id.ToString() == employeeId);
            return employee.HolidayDaysRemaining;
        }

        public async Task<bool> CheckIfEmployeeHasEnoughHolidayDaysAsync(string employeeId, int days)
        {
            var employee = await this.data.Employees.FirstAsync(e => e.Id.ToString() == employeeId);
            return employee.HolidayDaysRemaining > days;
        }

        public async Task<EmployeeServiceModel> GetEmployeeProfileDataAsync(string userId)
            => await this.data.Employees
                .Where(e => e.UserId == userId)
                .Select(e => new EmployeeServiceModel()
                {
                    Id = e.Id.ToString(),
                    FullName = $"{e.User.FirstName} {e.User.LastName}",
                    SupervisorName = $"{e.Supervisor.User.FirstName} {e.Supervisor.User.LastName}",
                    HolidayDaysRemaining = e.HolidayDaysRemaining
                })
                .FirstAsync();

        public async Task<EmployeeDetailsServiceModel> GetEmployeeDetailsAsync(string employeeId)
            => await this.data.Employees
                .Where(e => e.Id.ToString() == employeeId)
                .Select(e => new EmployeeDetailsServiceModel()
                {
                    Id = e.Id.ToString(),
                    FirstName = e.User.FirstName,
                    LastName = e.User.LastName,
                    Email = e.User.Email,
                    HolidayDaysRemaining = e.HolidayDaysRemaining
                })
                .FirstAsync();

        public async Task<List<EmployeeServiceModel>> GetSupervisorEmployeesAsync(string supervisorId)
            => await this.data.Employees
                .Where(e => e.SupervisorId.ToString() == supervisorId)
                .Select(e => new EmployeeServiceModel()
                {
                    Id = e.Id.ToString(),
                    FullName = $"{e.User.FirstName} {e.User.LastName}",
                })
                .ToListAsync();

        public async Task CreateEmployeeAsync(string firstName, string lastName,
            string email, string password, string supervisorId, string employeeRoleName)
        {
            var newUserId = await this.users.CreateUserAndReturnIdAsync(firstName, lastName, email, password);

            await this.users.AddUserToRoleAsync(newUserId, employeeRoleName);

            var newEmployee = new Employee()
            {
                HolidayDaysRemaining = InitialHolidayDaysCount,
                UserId = newUserId,
                SupervisorId = Guid.Parse(supervisorId)
            };

            await this.data.Employees.AddAsync(newEmployee);
            await this.data.SaveChangesAsync();
        }

        public async Task SubtractEmployeeHolidayDaysAsync(string employeeId, int days)
        {
            var employee = await this.data.Employees.FirstAsync(e => e.Id.ToString() == employeeId);

            employee.HolidayDaysRemaining -= days;
            await this.data.SaveChangesAsync();
        }

        public async Task EditEmployeeAsync(string employeeId, string firstName, string lastName,
            string email, string? newPassword)
        {
            var employee = await this.data.Employees
                .Include(e => e.User)
                .FirstAsync(e => e.Id.ToString() == employeeId);

            employee.User.FirstName = firstName;
            employee.User.LastName = lastName;

            await this.users.UpdateEmailAsync(employee.UserId, email);

            if (newPassword is not null)
            {
                await this.users.UpdatePasswordAsync(employee.UserId, newPassword);
            }

            await this.data.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(string employeeId)
        {
            var employee = await this.data.Employees.FirstAsync(e => e.Id.ToString() == employeeId);

            await this.requests.DeleteEmployeeRequestsAsync(employee.Id.ToString());

            this.data.Employees.Remove(employee);

            await this.users.DeleteUserAsync(employee.UserId);

            await this.data.SaveChangesAsync();
        }

        public async Task DeleteEmployeesBySupervisorIdAsync(string supervisorId)
        {
            var supervisor = await this.data.Supervisors.Include(s => s.Employees)
                .FirstAsync(s => s.Id.ToString() == supervisorId);

            foreach (var employee in supervisor.Employees)
            {
                await this.requests.DeleteEmployeeRequestsAsync(employee.Id.ToString());
            }

            var employeeUserIds = supervisor.Employees.Select(e => e.UserId).ToList();

            this.data.Employees.RemoveRange(supervisor.Employees);

            foreach (var userId in employeeUserIds)
            {
                await this.users.DeleteUserAsync(userId);
            }

            await this.data.SaveChangesAsync();
        }
    }
}
