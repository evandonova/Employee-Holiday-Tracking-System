using Microsoft.AspNetCore.Identity;
using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;

namespace EmployeeHolidayTrackingSystem.UnitTests
{
    public class DatabaseSeeder
    {
        public static HolidayRequestStatus PendingStatus = null!;
        public static HolidayRequestStatus ApprovedStatus = null!;
        public static HolidayRequestStatus DisapprovedStatus = null!;
        public static Employee TestEmployee = null!;
        public static Supervisor TestSupervisor = null!;
        public static HolidayRequest TestRequest = null!;
        public static IdentityRole TestRole = null!;

        public static void SeedDatabase(EmployeeHolidayDbContext data)
        {
            var employeeUser = new User()
            {
                UserName = "emp@hello.com",
                NormalizedUserName = "emp@HELLO.COM",
                Email = "EMP@hello.com",
                NormalizedEmail = "EMP@HELLO.COM",
                PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92",
                FirstName = "Greta",
                LastName = "Peterson"
            };

            var supervisorUser = new User()
            {
                UserName = "sup@hello.com",
                NormalizedUserName = "SUP@HELLO.COM",
                Email = "sup@hello.com",
                NormalizedEmail = "SUP@HELLO.COM",
                PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c93",
                FirstName = "Teo",
                LastName = "Tompson"
            };

            TestSupervisor = new Supervisor()
            {
                UserId = supervisorUser.Id
            };

            TestEmployee = new Employee()
            {
                UserId = employeeUser.Id,
                SupervisorId = TestSupervisor.Id,
                HolidayDaysRemaining = 20
            };

            PendingStatus = new HolidayRequestStatus()
            {
                Id = 1,
                Title = RequestStatusEnum.Pending.ToString()
            };
            ApprovedStatus = new HolidayRequestStatus()
            {
                Id = 2,
                Title = RequestStatusEnum.Approved.ToString()
            };
            DisapprovedStatus = new HolidayRequestStatus()
            {
                Id = 3,
                Title = RequestStatusEnum.Disapproved.ToString()
            };

            TestRequest = new HolidayRequest()
            {
                StartDate = DateTime.UtcNow.AddDays(5),
                EndDate = DateTime.UtcNow.AddDays(10),
                EmployeeId = TestEmployee.Id,
                SupervisorId = TestSupervisor.Id,
                StatusId = DisapprovedStatus.Id,
                DisapprovalStatement = "Some disapproval statement."
            };

            TestRole = new IdentityRole()
            {
                Id = "1",
                Name = "Tester"
            };

            data.Users.Add(employeeUser);
            data.Users.Add(supervisorUser);
            data.Employees.Add(TestEmployee);
            data.Supervisors.Add(TestSupervisor);
            data.HolidayRequestStatuses.Add(PendingStatus);
            data.HolidayRequestStatuses.Add(ApprovedStatus);
            data.HolidayRequestStatuses.Add(DisapprovedStatus);
            data.HolidayRequests.Add(TestRequest);
            data.Roles.Add(TestRole);

            data.SaveChanges();
        }
    }
}
