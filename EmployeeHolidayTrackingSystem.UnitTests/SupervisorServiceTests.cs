using Microsoft.EntityFrameworkCore;
using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;
using EmployeeHolidayTrackingSystem.Services.Users;
using EmployeeHolidayTrackingSystem.Services.Requests;
using EmployeeHolidayTrackingSystem.Services.Employees;
using EmployeeHolidayTrackingSystem.Services.Supervisors;
using EmployeeHolidayTrackingSystem.Services.RequestStatuses;

using static EmployeeHolidayTrackingSystem.UnitTests.DatabaseSeeder;

namespace EmployeeHolidayTrackingSystem.UnitTests
{
    public class SupervisorServiceTests
    {
        private DbContextOptions<EmployeeHolidayDbContext> dbOptions;
        private EmployeeHolidayDbContext data;

        private IUserService users;
        private IRequestService requests;
        private IEmployeeService employees;
        private IRequestStatusService statuses;
        private ISupervisorService supervisors;

        private string existingSupervisorId;

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
            this.supervisors = new SupervisorService(this.data, this.users, this.employees);

            existingSupervisorId = TestSupervisor.Id.ToString();
        }

        [Test]
        public async Task GetSupervisorIdByUserIdShouldReturnCorrectId()
        {
            var existingSupervisorUserId = TestSupervisor.User.Id.ToString();

            var resultSupervisorId = await this.supervisors.GetSupervisorIdByUserIdAsync(existingSupervisorUserId);

            Assert.That(resultSupervisorId, Is.EqualTo(TestSupervisor.Id.ToString()));
        }

        [Test]
        public async Task GetSupervisorFullNameShouldReturnCorrectName()
        {
            var resultFullName = await this.supervisors.GetSupervisorFullNameAsync(this.existingSupervisorId);

            Assert.That(resultFullName, Is.EqualTo($"{TestSupervisor.User.FirstName} {TestSupervisor.User.LastName}"));
        }

        [Test]
        public async Task SupervisorExistsShouldReturnTrueWhenExists()
        {
            var supervisorExists = await this.supervisors.SupervisorExistsAsync(this.existingSupervisorId);

            Assert.IsTrue(supervisorExists);
        }

        [Test]
        public async Task GetSupervisorEmailAddressShouldReturnCorrectEmail()
        {
            var resultEmail = await this.supervisors.GetSupervisorEmailAsync(this.existingSupervisorId);

            Assert.That(resultEmail, Is.EqualTo(TestSupervisor.User.Email));
        }

        [Test]
        public async Task GetSupervisorDetailsShouldReturnCorrectData()
        {
            var resultModel = await this.supervisors.GetSupervisorDetailsAsync(this.existingSupervisorId);

            Assert.That(resultModel.Id, Is.EqualTo(TestSupervisor.Id.ToString()));
            Assert.That(resultModel.FirstName, Is.EqualTo(TestSupervisor.User.FirstName));
            Assert.That(resultModel.LastName, Is.EqualTo(TestSupervisor.User.LastName));
            Assert.That(resultModel.Email, Is.EqualTo(TestSupervisor.User.Email));
        }

        [Test]
        public async Task GetAllSupervisorsShouldReturnCorrectData()
        {
            var supervisorsCount = this.data.Supervisors.Count();

            var result = await this.supervisors.GetAllSupervisorsAsync();
            var resultCount = result.Count();

            Assert.That(resultCount, Is.EqualTo(supervisorsCount));
        }

        [Test]
        public async Task CreateSupervisorShouldWorkCorrectly()
        {
            var firstName = "Test";
            var lastName = "Testov";
            var email = "test@test.com";

            var supervisorsCount = this.data.Supervisors.Count();
            var usersCount = this.data.Users.Count();
            var usersInRoleCount = this.data.UserRoles.Where(r => r.RoleId == TestRole.Id).Count();

            await this.supervisors.CreateSupervisorAsync(firstName, lastName, email,
                "pass123#", TestRole.Name);

            var newSupervisorsCount = this.data.Supervisors.Count();
            var newUsersCount = this.data.Users.Count();
            var newUsersInRoleCount = this.data.UserRoles.Where(r => r.RoleId == TestRole.Id).Count();

            Assert.That(newSupervisorsCount, Is.EqualTo(supervisorsCount + 1));
            Assert.That(newUsersCount, Is.EqualTo(usersCount + 1));
            Assert.That(newUsersInRoleCount, Is.EqualTo(usersInRoleCount + 1));

            var newSupervisor = this.data.Supervisors.First(e => e.Id.ToString() != TestSupervisor.Id.ToString());
            Assert.That(newSupervisor.User.FirstName, Is.EqualTo(firstName));
            Assert.That(newSupervisor.User.LastName, Is.EqualTo(lastName));
            Assert.That(newSupervisor.User.Email, Is.EqualTo(email));
        }

        [Test]
        public async Task EditSupervisorShouldWorkCorrectly()
        {
            var newSupervisor = new Supervisor()
            {
                User = new User()
                {
                    Email = "test@email.com",
                    FirstName = "First",
                    LastName = "Last"
                }
            };

            this.data.Supervisors.Add(newSupervisor);
            this.data.SaveChanges();

            var firstNameChanged = "Test";
            var lastNameChanged = "Testov";
            var emailChanged = "test@test.com";

            await this.supervisors.EditSupervisorAsync(newSupervisor.Id.ToString(), firstNameChanged, lastNameChanged, emailChanged, null);

            var supervisor = this.data.Supervisors.First(e => e.Id == newSupervisor.Id);
            Assert.IsNotNull(supervisor);
            Assert.That(supervisor.User.FirstName, Is.EqualTo(firstNameChanged));
            Assert.That(supervisor.User.LastName, Is.EqualTo(lastNameChanged));
            Assert.That(supervisor.User.Email, Is.EqualTo(emailChanged));
        }

        [Test]
        public async Task DeleteSupervisorShouldWorkCorrectly()
        {
            var newSupervisor = new Supervisor()
            {
                User = new User()
                {
                    Email = "test@email.com",
                    FirstName = "First",
                    LastName = "Last"
                },
                Employees = new List<Employee>
                {
                    new Employee()
                    {
                        User = new User()
                        {
                            FirstName = "User",
                            LastName = "User"
                        }
                    }
                }
            };

            this.data.Supervisors.Add(newSupervisor);
            this.data.SaveChanges();

            var usersCountBefore = this.data.Users.Count();
            var employeesCountBefore = this.data.Employees.Count();
            var supervisorsCountBefore = this.data.Supervisors.Count();

            await this.supervisors.DeleteSupervisorAsync(newSupervisor.Id.ToString());

            var usersCountAfter = this.data.Users.Count();
            var employeesCountAfter = this.data.Employees.Count();
            var supervisorsCountAfter = this.data.Supervisors.Count();

            var supervisor = this.data.Supervisors.FirstOrDefault(e => e.Id == newSupervisor.Id);
            Assert.IsNull(supervisor);
            Assert.That(usersCountAfter, Is.EqualTo(usersCountBefore - 2));
            Assert.That(employeesCountAfter, Is.EqualTo(employeesCountBefore - 1));
            Assert.That(supervisorsCountAfter, Is.EqualTo(supervisorsCountBefore - 1));
        }
    }
}
