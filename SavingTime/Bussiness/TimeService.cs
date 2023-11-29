using SavingTime.Data;
using SavingTime.Entities;

namespace SavingTime.Bussiness
{
    public class TimeService
    {
        private readonly SavingTimeDbContext dbContext;
        private readonly IssueService issueService;

        public TimeService(SavingTimeDbContext dbContext, IssueService issueService)
        {
            this.dbContext = dbContext;
            this.issueService = issueService;
        }

        public void Add(TimeRecord timeRecord)
        {
            using var transaction = dbContext.Database.BeginTransaction();
            try
            {
                addTimeRecord(timeRecord);

                var lastIssue = issueService.LastIssue();

                if (lastIssue is not null)
                {
                    issueService.Add(new IssueRecord(
                        timeRecord.Time,
                        timeRecord.Type,
                        lastIssue.Issue)
                    );
                }
                else if (!string.IsNullOrEmpty(timeRecord.Context))
                {
                    issueService.Entry(timeRecord.Time, timeRecord.Context);
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
            var context = timeRecord.Context is not null ? $"[{timeRecord.Context}]" : "";
            dbContext.Add(timeRecord);
            dbContext.SaveChanges();
            Console.WriteLine($"{timeRecord.Type} Registered {context}");
        }

        public bool IsWorking()
        {
            return dbContext.TimeRecords
                .OrderByDescending(t => t.Time)
                .ThenByDescending(t => t.Id)
                .First().Type == Entities.TimeRecordType.Entry;
        }
    }
}
