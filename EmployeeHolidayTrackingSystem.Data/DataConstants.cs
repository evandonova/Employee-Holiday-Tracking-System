namespace EmployeeHolidayTrackingSystem.Data
{
    public class DataConstants
    {
        public class User
        {
            public const int FirstNameMaxLength = 20;
            public const int LastNameMaxLength = 30;
        }

        public class Employee
        {
            public const int InitialHolidayDaysCount = 20;
        }

        public class HolidayRequest
        {
            public const int DisapprovalStatementMaxLength = 500;
        }

        public class HolidayRequestStatus
        {
            public const int TitleMaxLength = 20;
        }

        public enum HolidayRequestStatusEnum
        {
            Pending = 1,
            Approved = 2,
            Disapproved = 3
        }
    }
}
