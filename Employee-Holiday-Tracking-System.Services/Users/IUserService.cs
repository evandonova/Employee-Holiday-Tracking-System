namespace EmployeeHolidayTrackingSystem.Services.Users
{
    public interface IUserService
    {
        public void UpdatePassword(string id, string newPassword);
    }
}
