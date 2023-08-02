namespace EmployeeHolidayTrackingSystem.Data
{
    public class DataConstants
    {
        public class User
        {
            public const int FirstNameMaxLength = 20;
            public const int FirstNameMinLength = 2;

            public const int LastNameMaxLength = 30;
            public const int LastNameMinLength = 3;

            public const int PasswordMaxLength = 30;
            public const int PasswordMinLength = 6;

            public const int EmailMaxLength = 30;
            public const int EmailMinLength = 8;
        }

        public class Employee
        {
            public const int InitialHolidayDaysCount = 20;
        }

        public class HolidayRequest
        {
            public const int DisapprovalStatementMaxLength = 500;
            public const int DisapprovalStatementMinLength = 20;
            public static string DateFormat = "d MMMM yyyy";
        }

        public class HolidayRequestStatus
        {
            public const int TitleMaxLength = 20;
        }
    }
}
