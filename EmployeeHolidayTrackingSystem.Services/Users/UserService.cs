using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;

namespace EmployeeHolidayTrackingSystem.Services.Users
{
    public class UserService : IUserService
    {
        private readonly EmployeeHolidayDbContext data;

        public UserService(EmployeeHolidayDbContext data)
            => this.data = data;

        public async Task<string> CreateUserAndReturnIdAsync(string firstName, string lastName, string email, string password)
        {
            var newUser = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                NormalizedEmail = email.ToUpper(),
                UserName = email,
                NormalizedUserName = email.ToUpper(),
            };

            var hasher = new PasswordHasher<User>();
            newUser.PasswordHash = hasher.HashPassword(newUser, password);

            await this.data.Users.AddAsync(newUser);
            await this.data.SaveChangesAsync();

            return newUser.Id;
        }

        public async Task<string> GetUserFullNameAsync(string userId)
        {
            var user = await this.data.Users.FirstAsync(u => u.Id == userId);
            return $"{user.FirstName} {user.LastName}";
        }

        public async Task<bool> UserWithEmailExistsAsync(string email)
            => await this.data.Users.AnyAsync(u => u.Email == email);

        public async Task AddUserToRoleAsync(string userId, string roleName)
        {
            var user = await this.data.Users.FirstAsync(u => u.Id == userId);
            var role = await this.data.Roles.FirstAsync(r => r.Name == roleName);

            var userInRole = new IdentityUserRole<string>()
            {
                UserId = userId,
                RoleId = role.Id
            };

            await this.data.UserRoles.AddAsync(userInRole);
            await this.data.SaveChangesAsync();
        }

        public async Task UpdatePasswordAsync(string userId, string newPassword)
        {
            var user = await this.data.Users.FirstAsync(x => x.Id == userId);

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, newPassword);

            await this.data.SaveChangesAsync();
        }

        public async Task UpdateEmailAsync(string userId, string newEmail)
        {
            var user = await this.data.Users.FirstAsync(x => x.Id == userId);

            user.Email = newEmail;
            user.NormalizedEmail = newEmail.ToUpper();

            user.UserName = newEmail;
            user.NormalizedUserName = newEmail.ToUpper();

            await this.data.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await this.data.Users.FirstAsync(x => x.Id == userId);

            this.data.Users.Remove(user);
            await this.data.SaveChangesAsync();
        }
    }
}
