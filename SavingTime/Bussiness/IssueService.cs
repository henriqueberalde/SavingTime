using SavingTime.Data;
using SavingTime.Entities;

namespace SavingTime.Bussiness
{
    public class IssueService
    {
        private readonly SavingTimeDbContext dbContext;

        public IssueService(SavingTimeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Entry(DateTime dateTime, string issue)
        {
            var lastIssue = LastIssue();

            if (lastIssue is not null && lastIssue.Type == TimeRecordType.Entry)
            {
                Add(new IssueRecord(dateTime, TimeRecordType.Exit, lastIssue.Issue));
            }

            Add(new IssueRecord(dateTime, TimeRecordType.Entry, issue));
        }

        public void Add(IssueRecord issue)
        {
            validateRecordsIntegrityForType(issue.Type);

            dbContext.Add(issue);
            dbContext.SaveChanges();
            Console.WriteLine($"Issue {issue.Issue} {issue.Type} Registered");
        }

        public void validateRecordsIntegrityForType(TimeRecordType type)
        {
            var lastIssue = LastIssue();

            if (lastIssue is not null && lastIssue.Type == type)
            {
                throw new RecordIntegrityViolationException($"Can't add an issue of type {type} because it was the last type added");
            }
        }

        public IssueRecord? LastIssue()
        {
            return dbContext.IssueRecords
                .OrderByDescending(i => i.Time)
                .ThenByDescending(i => i.Id)
                .FirstOrDefault();
        }

        public void Summary(List<IssueRecord> list)
        {
            foreach (var groupItem in list.GroupBy(r => new { r.Time.Date, r.Issue }))
            {
                var slices = new List<TimeSpan>();
                var items = groupItem.OrderBy(r => r.Time).ThenByDescending(r => r.Id);
                DateTime? currentBeginDateTime = null;

                foreach (var record in items)
                {
                    if (record.Type == TimeRecordType.Entry)
                    {
                        currentBeginDateTime = record.Time;
                    }
                    else
                    {
                        if (!currentBeginDateTime.HasValue)
                            throw new Exception($"Error to process record {record.Time:yyyy/MM/dd} on day {groupItem.Key:yyyy/MM/dd}.");

                        var timeSpan = record.Time - currentBeginDateTime.Value;
                        slices.Add(timeSpan);
                    }
                }

                var a = new TimeSpan();
                var b = new TimeSpan();
                var c = a + b;
                TimeSpan totalInTimeSpan = new TimeSpan();
                slices.ForEach(s => totalInTimeSpan += s);
                decimal totalHoursInDecimal = (decimal)slices.Sum(s => s.TotalHours);

                Console.WriteLine($"{groupItem.Key.Date.ToString("yyyy/MM/dd")} ({groupItem.Key.Issue}) - {totalInTimeSpan.Hours}:{totalInTimeSpan.Minutes}:{totalInTimeSpan.Seconds} - {totalHoursInDecimal.ToString("N2")}");
            }
        }
    }
}
