using SavingTime.Data;
using SavingTime.Entities;

namespace SavingTime.Bussiness
{
    public class IssueService
    {
        private readonly SavingTimeDbContext dbContext;
        private readonly TimeService timeService;

        public IssueService(SavingTimeDbContext dbContext, TimeService timeService)
        {
            this.dbContext = dbContext;
            this.timeService = timeService;
        }

        public void AddIssueEntry(DateTime dateTime, string issue)
        {
            if (!timeService.IsWorking())
            {
                throw new Exception("Cannot add issue while out of work");
            }
            var lastIssue = lastIssueRecord();

            if (lastIssue is not null && lastIssue.Type == TimeRecordType.Entry)
            {
                addIssue(new IssueRecord(dateTime, TimeRecordType.Exit, lastIssue.Issue));
            }

            addIssue(new IssueRecord(dateTime, TimeRecordType.Entry, issue));
        }

        public void AddIssueExit(DateTime dateTime, string issue)
        {
            if (string.IsNullOrEmpty(issue))
            {
                throw new Exception("issue must have value");
            }

            addIssue(new IssueRecord(dateTime, TimeRecordType.Exit, issue));
        }

        private void addIssue(IssueRecord issue)
        {
            dbContext.Add(issue);
            dbContext.SaveChanges();
            Console.WriteLine($"Issue {issue} {issue.Type.ToString()} Registered");
        }

        private IssueRecord? lastIssueRecord()
        {
            return dbContext.IssueRecords.OrderByDescending(t => t.Time).FirstOrDefault();
        }
    }
}
