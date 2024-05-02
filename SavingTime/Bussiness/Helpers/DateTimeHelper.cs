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

        public static DateTime LastTimeOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

        public static string FormatTimeSpan(TimeSpan t)
        {
            var hours = (int)t.TotalHours;
            var minutes = t.Minutes;

            var hoursStr = Math.Abs(hours).ToString("00");
            var minutesStr = Math.Abs(minutes).ToString("00");
            var finalStr = $"{hoursStr}:{minutesStr}";

            if (t.TotalHours < 0)
                return $"-{finalStr}";
            else
                return $" {finalStr}";
        }
    }
}
