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
    }
}
