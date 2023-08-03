namespace EmployeeHolidayTrackingSystem.Services.Users
{
    public interface IUserService
    {
        public Task<string> CreateUserAndReturnIdAsync(string firstName, string lastName, string email, string password);

        public Task<string> GetUserFullNameAsync(string userId);

        public Task<bool> UserWithEmailExistsAsync(string email);

        public Task AddUserToRoleAsync(string userId, string roleName);

        public Task UpdatePasswordAsync(string id, string newPassword);

        public Task UpdateEmailAsync(string id, string newEmail);

        public Task DeleteUserAsync(string id);
    }
}
