using SavingTime.Data;
using SavingTime.Entities;

namespace SavingTime.Business
{
    public class TimeRecordService
    {
        private readonly SavingTimeDbContext dbContext;
        private readonly IssueService issueService;

        public TimeRecordService(SavingTimeDbContext dbContext, IssueService issueService)
        {
            this.dbContext = dbContext;
            this.issueService = issueService;
        }

        public void Add(TimeRecord timeRecord, string? issue)
        {
            using var transaction = dbContext.Database.BeginTransaction();
            try
            {
                addTimeRecord(timeRecord);

                var lastIssue = issueService.LastIssue();

                if (lastIssue is not null)
                {
                    var issueToSave = lastIssue.Issue;

                    if (timeRecord.Type == TimeRecordType.Entry && !string.IsNullOrEmpty(issue))
                        issueToSave = issue;

                    issueService.Add(new IssueRecord(
                        timeRecord.Time,
                        timeRecord.Type,
                        issueToSave)
                    );
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void addTimeRecord(TimeRecord timeRecord)
        {
            dbContext.Add(timeRecord);
            dbContext.SaveChanges();
            Console.WriteLine($"{timeRecord.Type} Registered");
        }

        public bool IsWorking()
        {
            return dbContext.TimeRecords
                .OrderByDescending(t => t.Time)
                .ThenByDescending(t => t.Id)
                .First().Type == Entities.TimeRecordType.Entry;
        }

        public TimeRecordType? LastType()
        {
            return LastType(dbContext.TimeRecords.ToList());
        }

        public TimeRecordType? LastType(List<TimeRecord> list)
        {
            var localList = list ?? dbContext.TimeRecords.ToList();

            return localList
                .OrderByDescending(r => r.Time)
                .ThenByDescending(t => t.Id)
                .FirstOrDefault()?.Type;
        }
    }
}
