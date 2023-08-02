using Microsoft.AspNetCore.Identity;
using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;

namespace EmployeeHolidayTrackingSystem.Services.Users
{
    public class UserService : IUserService
    {
        private readonly EmployeeHolidayDbContext data;

        public UserService(EmployeeHolidayDbContext data)
            => this.data = data;

        public string CreateUser(string firstName, string lastName, string email, string password)
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

            this.data.Users.Add(newUser);
            this.data.SaveChanges();

            return newUser.Id;
        }

        public bool UserWithEmailExists(string email)
            => this.data.Users.Any(u => u.Email == email);

        public void AddUserToRole(string userId, string roleName)
        {
            var user = this.data.Users.Find(userId);

            if (user is null)
            {
                return;
            }

            var role = this.data.Roles.FirstOrDefault(r => r.Name == roleName);

            if (role is null)
            {
                return;
            }

            var userInRole = new IdentityUserRole<string>()
            {
                UserId = userId,
                RoleId = role.Id
            };

            this.data.UserRoles.Add(userInRole);
            this.data.SaveChanges();
        }

        public void UpdatePassword(string id, string newPassword)
        {
            var user = this.data.Users.FirstOrDefault(x => x.Id == id);

            if (user is not null) 
            {
                var hasher = new PasswordHasher<User>();
                user.PasswordHash = hasher.HashPassword(user, newPassword);

                this.data.SaveChanges();
            }
        }

        public void UpdateEmail(string id, string newEmail)
        {
            var user = this.data.Users.FirstOrDefault(x => x.Id == id);

            if (user is not null)
            {
                user.Email = newEmail;
                user.NormalizedEmail = newEmail.ToUpper();

                user.UserName = newEmail;
                user.NormalizedUserName = newEmail.ToUpper();

                this.data.SaveChanges();
            }
        }

        public void DeleteUser(string id)
        {
            var user = this.data.Users.FirstOrDefault(x => x.Id == id);

            if (user is not null)
            {
                this.data.Users.Remove(user);
                this.data.SaveChanges();
            }
        }
    }
}
