namespace EmployeeHolidayTrackingSystem.Services.Users
{
    public interface IUserService
    {
        public void UpdatePassword(string id, string newPassword);

        public void UpdateEmail(string id, string newEmail);
    }
}
