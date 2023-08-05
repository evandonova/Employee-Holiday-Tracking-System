namespace EmployeeHolidayTrackingSystem.Services.Users
{
    public interface IUserService
    {
        Task<string> CreateUserAndReturnIdAsync(string firstName, string lastName, string email, string password);

        Task<string> GetUserFullNameAsync(string userId);

        Task<bool> UserWithEmailExistsAsync(string email);

        Task AddUserToRoleAsync(string userId, string roleName);

        Task UpdatePasswordAsync(string id, string newPassword);

        Task UpdateEmailAsync(string id, string newEmail);

        Task DeleteUserAsync(string id);
    }
}
