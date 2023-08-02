namespace EmployeeHolidayTrackingSystem.Services.Users
{
    public interface IUserService
    {
        public string CreateUser(string firstName, string lastName, string email, string password);

        public string GetUserFullName(string id);

        public bool UserWithEmailExists(string email);

        public void AddUserToRole(string userId, string roleName);

        public void UpdatePassword(string id, string newPassword);

        public void UpdateEmail(string id, string newEmail);

        public void DeleteUser(string id);
    }
}
