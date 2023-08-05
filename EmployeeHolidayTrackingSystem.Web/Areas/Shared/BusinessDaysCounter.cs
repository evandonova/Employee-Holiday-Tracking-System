namespace EmployeeHolidayTrackingSystem.Web.Areas.Shared
{
    public static class BusinessDaysCounter
    {
        public static int GetDaysCount(DateTime startDate, DateTime endDate)
        {
            double calcBusinessDays =
                1 + ((endDate - startDate).TotalDays * 5 -
                (startDate.DayOfWeek - endDate.DayOfWeek) * 2) / 7;

            return (int)calcBusinessDays;
        }
    }
}
