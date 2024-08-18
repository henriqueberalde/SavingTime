using SavingTime.Bussiness.Helpers;
using SavingTime.Entities;
using System.Text;

namespace SavingTime.Bussiness
{
    public class SummaryItem
    {
        public DateTime Date { get; set; }

        public List<TimeRecord> TimeRecords { get; private set; }

        public TimeSpan Total {
            get
            {
                var total = new TimeSpan();

                foreach (var slice in WorkedSlices) { total += slice.Total; }

                return total;
            }
        }

        public TimeSpan Diff
        {
            get
            {
                if (Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    return Total;
                }
                else {
                    return TimeSpan.FromHours(-8) + Total;
                }
            }
        }

        public int? MinHour
        {
            get
            {
                int? min = null;

                foreach (var t in TimeRecords)
                {
                    if (min is null || min > t.Time.Hour) { min = t.Time.Hour; }
                }

                return min;
            }
        }

        public int? MaxHour
        {
            get
            {
                int? max = null;

                foreach (var t in TimeRecords)
                {
                    if (max is null || max < t.Time.Hour) { max = t.Time.Hour; }
                }

                return max;
            }
        }

        private List<WorkedSlice> WorkedSlices { get; set; }
        private List<string> Fractions { get; set; }

        public SummaryItem(
            DateTime date,
            List<TimeRecord> timeRecords)
        {
            Date = date;
            TimeRecords = timeRecords;

            WorkedSlices = WorkedSliceList();
            Fractions = TimeFractions();
        }

        public override string ToString()
        {
            if (MinHour is null || MaxHour is null)
                throw new Exception("Min or Max null error");

            return ToString(MinHour.Value, MaxHour.Value);
        }

        public string ToString(int min, int max)
        {
            var result = new StringBuilder();

            result.Append($"{Date.ToString("dd/MM")} |");

            for (int i = min; i <= max; i++)
            {
                for (int j = 1; j <= 6; j++)
                {
                    if (Fractions.Contains($"{i.ToString("D2")}_{j}"))
                    {
                        result.Append("\u2588");
                        continue;
                    }

                    result.Append(" ");
                }
            }
            var diffStr = DateTimeHelper.FormatTimeSpan(Diff);
            result.Append($"| {Total.Hours.ToString("D2")}:{Total.Minutes.ToString("D2")} ({string.Format("{0:00.00}", (decimal)Total.TotalHours)}) | {diffStr}");

            return result.ToString();
        }

        public static SummaryItem FromTimeRecordList(List<TimeRecord> list)
        {
            var first = list[0];
            return new SummaryItem(first.Time, list);
        }

        private List<WorkedSlice> WorkedSliceList()
        {
            ValidateTimeRecordList();

            var result = new List<WorkedSlice>();
            var begin = "";

            foreach (var timeRecord in TimeRecords)
            {
                var hour = timeRecord.Time.Hour.ToString("D2");
                var minutes = timeRecord.Time.Minute.ToString("D2");
                var currentTime = $"{hour}:{minutes}";

                if (timeRecord.Type == TimeRecordType.Entry)
                {
                    begin = currentTime;
                    continue;
                }

                result.Add(new WorkedSlice(begin, currentTime));
            }

            return result;
        }

        private void ValidateTimeRecordList()
        {
            var entryCount = TimeRecords.Count(i => i.Type == TimeRecordType.Entry);
            var exitCount = TimeRecords.Count(i => i.Type == TimeRecordType.Exit);

            var valid = (TimeRecords.Count % 2 == 0) &&
                (entryCount == exitCount) &&
                (TimeRecords.First().Type == TimeRecordType.Entry) &&
                (TimeRecords.Last().Type == TimeRecordType.Exit);

            if (!valid) { throw new Exception("Invalid Records"); }
        }

        public List<string> TimeFractions()
        {
            var fractions = new List<string>();

            WorkedSlices.ForEach(slice => fractions.AddRange(slice.ToFractions()));

            return fractions;
        }
    }
}
