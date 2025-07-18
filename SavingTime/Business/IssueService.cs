using SavingTime.Data;
using SavingTime.Entities;

namespace SavingTime.Business
{
    public class IssueService
    {
        private readonly SavingTimeDbContext dbContext;

        public IssueService(SavingTimeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Entry(DateTime dateTime, string issue, string? comment = null)
        {
            var lastIssue = LastIssue(dateTime);

            if (lastIssue is not null && lastIssue.Type == TimeRecordType.Entry)
            {
                Add(new IssueRecord(dateTime, TimeRecordType.Exit, lastIssue.Issue, lastIssue.Comment));
            }

            Add(new IssueRecord(dateTime, TimeRecordType.Entry, issue, comment));
        }

        public void Add(IssueRecord issue)
        {
            validateRecordsIntegrityForType(issue.Type);

            dbContext.Add(issue);
            dbContext.SaveChanges();

            var withDateMessage = "";

            if (issue.Time.Day != DateTime.Now.Day) {
                withDateMessage = $" on {issue.Time:dd/MM/yyyy HH:mm}";
            } else if ((DateTime.Now - issue.Time).TotalSeconds > 60) {
                withDateMessage = $" at {issue.Time:HH:mm}";
            }

            Console.WriteLine($"Issue {issue.Issue} {issue.Type} Registered{withDateMessage}");
        }

        public void validateRecordsIntegrityForType(TimeRecordType type)
        {
            var lastIssue = LastIssue();

            if (lastIssue is not null && lastIssue.Type == type)
            {
                throw new RecordIntegrityViolationException($"Can't add an issue of type {type} because it was the last type added");
            }
        }

        public IssueRecord? LastIssue(DateTime? refDateTime = null)
        {
            var dateTime = refDateTime ?? DateTime.Now;
            return dbContext.IssueRecords
                .Where(i => i.Time <= dateTime)
                .OrderByDescending(i => i.Time)
                .ThenByDescending(i => i.Id)
                .FirstOrDefault();
        }

        public void Summary(List<IssueRecord> list)
        {
            if (!list.Any())
            {
                Console.WriteLine("\nNo records");
                return;
            }

            var descriptionMaxlength = list.Select(i => i.Issue.Length).Max();
            foreach (var groupItem in list.GroupBy(r => new { r.Time.Date, r.Issue, r.Comment }))
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
                            throw new Exception($"Error to process record {record.Time:yyyy/MM/dd} on day {groupItem.Key.Date:yyyy/MM/dd}.");

                        var timeSpan = record.Time - currentBeginDateTime.Value;
                        slices.Add(timeSpan);
                    }
                }

                TimeSpan totalInTimeSpan = new TimeSpan();
                slices.ForEach(s => totalInTimeSpan += s);
                decimal totalHoursInDecimal = (decimal)slices.Sum(s => s.TotalHours);
                var issueLenght = groupItem.Key.Issue.Length;
                var issueStr = $"{groupItem.Key.Issue}".PadRight(descriptionMaxlength, ' ');
                var commentStr = string.IsNullOrEmpty(groupItem.Key.Comment) ? "": groupItem.Key.Comment;
                commentStr = commentStr.Length <= 8 ? commentStr : commentStr.Substring(0, 8) + "...";
                commentStr = commentStr.PadRight(11, ' ');

                Console.WriteLine($"{groupItem.Key.Date.ToString("dd/MM")} | {issueStr} {commentStr} - {totalInTimeSpan.Hours:D2}:{totalInTimeSpan.Minutes:D2} (In hour: {totalHoursInDecimal:00.00}) (In sec: {totalInTimeSpan.TotalSeconds:000000})");
            }
        }
    }
}
