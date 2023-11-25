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
            var lastIssue = lastIssueRecord();

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
                .FirstOrDefault();
        }

        private IssueRecord? lastIssueRecord()
        {
            return dbContext.IssueRecords.OrderByDescending(t => t.Time).FirstOrDefault();
        }
    }
}
