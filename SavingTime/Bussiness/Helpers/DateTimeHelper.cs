namespace SavingTime.Bussiness.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime LastDayOfMonth(DateTime date)
        {
            return FirstDayOfMonth(date.AddMonths(1)).AddDays(-1);
        }

        public static DateTime FirstDayOfMonth(DateTime date)
        {
            var year = date.Year;
            var month = date.Month;
            return new DateTime(year, month, 1);
        }

        public static DateTime LastMonth()
        {
            return DateTime.Now.AddMonths(-1);
        }
    }
}
