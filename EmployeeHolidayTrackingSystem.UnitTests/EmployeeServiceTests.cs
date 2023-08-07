using Microsoft.EntityFrameworkCore;
using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;
using EmployeeHolidayTrackingSystem.Services.Users;
using EmployeeHolidayTrackingSystem.Services.Requests;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Services.RequestStatuses;
using EmployeeHolidayTrackingSystem.Services.Employees.Models;

using static EmployeeHolidayTrackingSystem.UnitTests.DatabaseSeeder;

namespace EmployeeHolidayTrackingSystem.UnitTests
{
    public class EmployeeServiceTests
    {
        private DbContextOptions<EmployeeHolidayDbContext> dbOptions;
        private EmployeeHolidayDbContext data;

        private IEmployeeService employees;
        private IUserService users;
        private IRequestService requests;
        private IRequestStatusService statuses;

        private string existingEmployeeId;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.dbOptions = new DbContextOptionsBuilder<EmployeeHolidayDbContext>()
                .UseInMemoryDatabase("EmployeeHolidayInMemoryDb" + Guid.NewGuid().ToString())
                .Options;

            this.data = new EmployeeHolidayDbContext(this.dbOptions);

            this.data.Database.EnsureCreated();
            SeedDatabase(this.data);

            this.statuses = new RequestStatusService(this.data);
            this.requests = new RequestService(this.data, this.statuses);
            this.users = new UserService(this.data);
            this.employees = new EmployeeService(this.data, this.users, this.requests);

            existingEmployeeId = TestEmployee.Id.ToString();
        }

        [Test]
        public async Task GetEmployeeIdByUserIdShouldReturnCorrectId()
        {
            var existingEmployeeUserId = TestEmployee.User.Id.ToString();

            var resultEmployeeId = await this.employees.GetEmployeeIdByUserIdAsync(existingEmployeeUserId);

            Assert.That(resultEmployeeId, Is.EqualTo(TestEmployee.Id.ToString()));
        }

        [Test]
        public async Task EmployeeExistsShouldReturnTrueWhenExists()
        {
            var employeeExists = await this.employees.EmployeeExistsAsync(this.existingEmployeeId);

            Assert.IsTrue(employeeExists);
        }

        [Test]
        public async Task GetEmployeeEmailAddressShouldReturnCorrectEmail()
        {
            var resultEmail = await this.employees.GetEmployeeEmailAsync(this.existingEmployeeId);

            Assert.That(resultEmail, Is.EqualTo(TestEmployee.User.Email));
        }

        [Test]
        public async Task GetEmployeeFullNameShouldReturnCorrectName()
        {
            var resultFullName = await this.employees.GetEmployeeFullNameAsync(this.existingEmployeeId);

            Assert.That(resultFullName, Is.EqualTo($"{TestEmployee.User.FirstName} {TestEmployee.User.LastName}"));
        }

        [Test]
        public async Task GetEmployeeSupervisorIdShouldReturnCorrectId()
        {
            var resultSupervisorId = await this.employees.GetEmployeeSupervisorIdAsync(this.existingEmployeeId);

            Assert.That(resultSupervisorId, Is.EqualTo(TestEmployee.Supervisor.Id.ToString()));
        }

        [Test]
        public async Task GetEmployeeHolidayDaysRemainingShouldReturnCorrectCount()
        {
            var resultDays = await this.employees.GetEmployeeSupervisorIdAsync(this.existingEmployeeId);

            Assert.That(resultDays, Is.EqualTo(TestEmployee.Supervisor.Id.ToString()));
        }

        [Test]
        public async Task CheckIfEmployeeHasEnoughHolidayDaysShouldReturnTrueWhenEnough()
        {
            var result = await this.employees.CheckIfEmployeeHasEnoughHolidayDaysAsync(this.existingEmployeeId, 10);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetEmployeeProfileDataByUserIdShouldReturnCorrectData()
        {
            var resultModel = await this.employees.GetEmployeeProfileDataByUserIdAsync(TestEmployee.User.Id.ToString());

            Assert.That(resultModel.Id, Is.EqualTo(TestEmployee.Id.ToString()));
            Assert.That(resultModel.FullName, Is.EqualTo($"{TestEmployee.User.FirstName} {TestEmployee.User.LastName}"));
            Assert.That(resultModel.HolidayDaysRemaining, Is.EqualTo(TestEmployee.HolidayDaysRemaining));
            Assert.That(resultModel.SupervisorName, Is.EqualTo($"{TestSupervisor.User.FirstName} {TestSupervisor.User.LastName}"));
        }

        [Test]
        public async Task GetEmployeeDetailsShouldReturnCorrectData()
        {
            var resultModel = await this.employees.GetEmployeeDetailsAsync(this.existingEmployeeId);

            Assert.That(resultModel.Id, Is.EqualTo(TestEmployee.Id.ToString()));
            Assert.That(resultModel.FirstName, Is.EqualTo(TestEmployee.User.FirstName));
            Assert.That(resultModel.LastName, Is.EqualTo(TestEmployee.User.LastName));
            Assert.That(resultModel.Email, Is.EqualTo(TestEmployee.User.Email));
            Assert.That(resultModel.HolidayDaysRemaining, Is.EqualTo(TestEmployee.HolidayDaysRemaining));
        }

        [Test]
        public async Task GetEmployeesBySupervisorIdShouldReturnCorrectData()
        {
            var supervisorEmployeesCount = this.data.Employees.Where(e => e.SupervisorId.ToString() == TestSupervisor.Id.ToString()).Count();
            var resultCollection = await this.employees.GetEmployeesBySupervisorIdAsync(TestSupervisor.Id.ToString());

            Assert.IsNotNull(resultCollection);
            Assert.That(resultCollection.Count, Is.EqualTo(supervisorEmployeesCount));

            var resultEmployee = resultCollection.First(e => e.Id == TestEmployee.Id.ToString());
            Assert.IsNotNull(resultEmployee);
            Assert.That(resultEmployee.Id, Is.EqualTo(TestEmployee.Id.ToString()));
            Assert.That(resultEmployee.FullName, Is.EqualTo($"{TestEmployee.User.FirstName} {TestEmployee.User.LastName}"));
        }

        [Test]
        public async Task CreateEmployeeShouldWorkCorrectly()
        {
            var firstName = "Test";
            var lastName = "Testov";
            var email = "test@test.com";

            var employeesCount = this.data.Employees.Count();
            var usersCount = this.data.Users.Count();
            var usersInRoleCount = this.data.UserRoles.Where(r => r.RoleId == TestRole.Id).Count();

            await this.employees.CreateEmployeeAsync(firstName, lastName, email,
                "pass123#", TestSupervisor.Id.ToString(), TestRole.Name);

            var newEmployeesCount = this.data.Employees.Count();
            var newUsersCount = this.data.Users.Count();
            var newUsersInRoleCount = this.data.UserRoles.Where(r => r.RoleId == TestRole.Id).Count();

            Assert.That(newEmployeesCount, Is.EqualTo(employeesCount + 1));
            Assert.That(newUsersCount, Is.EqualTo(usersCount + 1));
            Assert.That(newUsersInRoleCount, Is.EqualTo(usersInRoleCount + 1));

            var newEmployee = this.data.Employees.First(e => e.Id.ToString() != TestEmployee.Id.ToString());
            Assert.That(newEmployee.User.FirstName, Is.EqualTo(firstName));
            Assert.That(newEmployee.User.LastName, Is.EqualTo(lastName));
            Assert.That(newEmployee.User.Email, Is.EqualTo(email));
        }

        [Test]
        public async Task SubtractEmployeeHolidayDaysShouldWorkCorrectly()
        {
            var currentEmployeeHolidayDays = TestEmployee.HolidayDaysRemaining;
            var subtractedDays = 5;

            await this.employees.SubtractEmployeeHolidayDaysAsync(this.existingEmployeeId, subtractedDays);

            var employee = this.data.Employees.First(e => e.Id == TestEmployee.Id);
            Assert.That(employee.HolidayDaysRemaining, Is.EqualTo(currentEmployeeHolidayDays - subtractedDays));
        }

        [Test]
        public async Task EditEmployeeShouldWorkCorrectly()
        {
            var newEmployee = new Employee()
            {
                User = new User()
                {
                    Email = "test@email.com",
                    FirstName = "First",
                    LastName = "Last"
                }
            };

            this.data.Employees.Add(newEmployee);
            this.data.SaveChanges();

            var firstNameChanged = "Test";
            var lastNameChanged = "Testov";
            var emailChanged = "test@test.com";

            await this.employees.EditEmployeeAsync(newEmployee.Id.ToString(), firstNameChanged, lastNameChanged, emailChanged, null);

            var employee = this.data.Employees.First(e => e.Id == newEmployee.Id);
            Assert.IsNotNull(employee);
            Assert.That(employee.User.FirstName, Is.EqualTo(firstNameChanged));
            Assert.That(employee.User.LastName, Is.EqualTo(lastNameChanged));
            Assert.That(employee.User.Email, Is.EqualTo(emailChanged));
        }

        [Test]
        public async Task DeleteEmployeeShouldWorkCorrectly()
        {
            var newEmployee = new Employee()
            {
                User = new User()
                {
                    Email = "test@email.com",
                    FirstName = "First",
                    LastName = "Last"
                }
            };

            this.data.Employees.Add(newEmployee);
            this.data.SaveChanges();

            var usersCountBefore = this.data.Users.Count();
            var employeesCountBefore = this.data.Employees.Count();

            await this.employees.DeleteEmployeeAsync(newEmployee.Id.ToString());

            var usersCountAfter = this.data.Users.Count();
            var employeesCountAfter = this.data.Employees.Count();

            var employee = this.data.Employees.FirstOrDefault(e => e.Id == newEmployee.Id);
            Assert.IsNull(employee);
            Assert.That(usersCountAfter, Is.EqualTo(usersCountBefore - 1));
            Assert.That(employeesCountAfter, Is.EqualTo(employeesCountBefore - 1));
        }

        [Test]
        public async Task DeleteEmployeesBySupervisorIdShouldWorkCorrectly()
        {
            var newSupervisor = new Supervisor()
            {
                User = new User()
                {
                    Email = "sup@email.com",
                    FirstName = "Super",
                    LastName = "Visor"
                },
            };

            var newEmployee = new Employee()
            {
                User = new User()
                {
                    Email = "test@email.com",
                    FirstName = "First",
                    LastName = "Last"
                },
                SupervisorId = newSupervisor.Id
            };

            this.data.Supervisors.Add(newSupervisor);
            this.data.Employees.Add(newEmployee);
            this.data.SaveChanges();

            var supervisorEmployeesCountBefore = this.data.Employees
                .Where(e => e.SupervisorId == newSupervisor.Id).Count();

            await this.employees.DeleteEmployeesBySupervisorIdAsync(newSupervisor.Id.ToString());

            var supervisorEmployeesCountAfter = this.data.Employees
                .Where(e => e.SupervisorId == newSupervisor.Id).Count();

            Assert.That(supervisorEmployeesCountAfter, Is.EqualTo(supervisorEmployeesCountBefore - 1));
        }
    }
}