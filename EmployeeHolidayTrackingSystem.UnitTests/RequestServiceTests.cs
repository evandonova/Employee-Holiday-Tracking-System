using System.Globalization;
using Microsoft.EntityFrameworkCore;
using EmployeeHolidayTrackingSystem.Data;
using EmployeeHolidayTrackingSystem.Data.Models;
using EmployeeHolidayTrackingSystem.Services.Requests;
using EmployeeHolidayTrackingSystem.Services.RequestStatuses;
using EmployeeHolidayTrackingSystem.Services.Requests.Models;

using static EmployeeHolidayTrackingSystem.UnitTests.DatabaseSeeder;

namespace EmployeeHolidayTrackingSystem.UnitTests
{
    public class RequestServiceTests
    {
        private const string customDateFormat = "d MMMM yyyy";

        private DbContextOptions<EmployeeHolidayDbContext> dbOptions;
        private EmployeeHolidayDbContext data;

        private IRequestService requests;
        private IRequestStatusService statuses;

        private string existingRequestId;

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

            this.existingRequestId = TestRequest.Id.ToString();
        }

        [Test]
        public async Task RequestExistsShouldReturnTrueWhenExists()
        {
            var requestExists = await this.requests.RequestExistsAsync(this.existingRequestId);

            Assert.IsTrue(requestExists);
        }

        [Test]
        public async Task GetRequestEmployeeIdShouldReturnCorrectId()
        {
            var resultEmployeeId = await this.requests.GetRequestEmployeeIdAsync(this.existingRequestId);

            Assert.That(resultEmployeeId, Is.EqualTo(TestRequest.Employee.Id.ToString()));
        }

        [Test]
        public async Task GetRequestStatusTitleShouldReturnCorrectTitle()
        {
            var resultStatusTitle = await this.requests.GetRequestStatusTitleAsync(this.existingRequestId);

            Assert.That(resultStatusTitle, Is.EqualTo(TestRequest.Status.Title));
        }

        [Test]
        public async Task GetRequestEmployeeFullNameShouldReturnCorrectName()
        {
            var resultEmployeeFullName = await this.requests.GetRequestEmployeeFullNameAsync(this.existingRequestId);

            Assert.That(resultEmployeeFullName, Is.EqualTo($"{TestRequest.Employee.User.FirstName} {TestRequest.Employee.User.LastName}"));
        }

        [Test]
        public async Task GetDisapprovalStatementShouldReturnCorrectText()
        {
            var resultDisapprovalText = await this.requests.GetRequestDisapprovalStatementAsync(this.existingRequestId);

            Assert.That(resultDisapprovalText, Is.EqualTo(TestRequest.DisapprovalStatement));
        }

        [Test]
        public async Task CreateShouldWorkCorrectly()
        {
            var startDate = DateTime.UtcNow.AddDays(1);
            var endDate = DateTime.UtcNow.AddDays(2);

            var requestsCountBefore = this.data.HolidayRequests.Count();

            await this.requests.CreateRequestAsync(startDate, endDate, TestEmployee.Id.ToString(), TestSupervisor.Id.ToString());

            var requestsCountAfter = this.data.HolidayRequests.Count();

            Assert.That(requestsCountAfter, Is.EqualTo(requestsCountBefore + 1));

            var newRequest = this.data.HolidayRequests.First(r => r.Id != TestRequest.Id);
            Assert.That(newRequest.StartDate, Is.EqualTo(startDate));
            Assert.That(newRequest.EndDate, Is.EqualTo(endDate));
            Assert.That(newRequest.Status.Title, Is.EqualTo(PendingStatus.Title));
            Assert.That(newRequest.EmployeeId, Is.EqualTo(TestEmployee.Id));
            Assert.That(newRequest.SupervisorId, Is.EqualTo(TestSupervisor.Id));
        }

        [Test]
        public async Task UpdateRequestToApprovedShouldWorkCorrectly()
        {
            await this.requests.UpdateRequestToApprovedAsync(this.existingRequestId);

            var changedRequest = this.data.HolidayRequests.First(r => r.Id == TestRequest.Id);

            Assert.That(changedRequest.Status, Is.EqualTo(ApprovedStatus));
        }

        [Test]
        public async Task UpdateDisapprovedRequestShouldWorkCorrectly()
        {
            var newDisapprovalStatement = "New disapproval statement";

            await this.requests.UpdateDisapprovedRequestAsync(this.existingRequestId, newDisapprovalStatement);

            var changedRequest = this.data.HolidayRequests.First(r => r.Id == TestRequest.Id);

            Assert.That(changedRequest.Status, Is.EqualTo(DisapprovedStatus));
            Assert.That(changedRequest.DisapprovalStatement, Is.EqualTo(newDisapprovalStatement));
        }

        [Test]
        public async Task DeleteEmployeeRequestsRequestShouldWorkCorrectly()
        {
            var newEmployeeWithRequests = new Employee()
            {
                UserId = "cd3e1ed5-8a35-42d4-ba2d-e736fb756217",
                HolidayRequests = new List<HolidayRequest>()
                {
                    new HolidayRequest() { },
                    new HolidayRequest() { },
                }
            };

            this.data.Employees.Add(newEmployeeWithRequests);
            this.data.SaveChanges();

            var employeeRequestsCountBefore = this.data.HolidayRequests
                .Where(r => r.EmployeeId == newEmployeeWithRequests.Id).Count();

            await this.requests.DeleteEmployeeRequestsAsync(newEmployeeWithRequests.Id.ToString());

            var employeeRequestsCountAfter = this.data.HolidayRequests
                .Where(r => r.EmployeeId == newEmployeeWithRequests.Id).Count();

            Assert.That(employeeRequestsCountAfter, Is.EqualTo(employeeRequestsCountBefore - 2));
        }

        [Test]
        public async Task GetRequestDetailsShouldReturnCorrectData()
        {
            var resultModel = await this.requests.GetRequestDetailsAsync(TestRequest.Id.ToString());

            Assert.That(resultModel.Id, Is.EqualTo(TestRequest.Id.ToString()));
            Assert.That(resultModel.StartDate, Is.EqualTo(TestRequest.StartDate.ToString(customDateFormat, CultureInfo.InvariantCulture)));
            Assert.That(resultModel.EndDate, Is.EqualTo(TestRequest.EndDate.ToString(customDateFormat, CultureInfo.InvariantCulture)));
        }

        [Test]
        public async Task GetEmployeeRequestsShouldReturnCorrectData()
        {
            var requestsCount = this.data.HolidayRequests.Where(r => r.EmployeeId == TestEmployee.Id).Count();

            var resultCount = await this.requests.GetEmployeeRequestsAsync(TestEmployee.Id.ToString());

            Assert.That(requestsCount, Is.EqualTo(resultCount.Count()));
        }
    }
}
