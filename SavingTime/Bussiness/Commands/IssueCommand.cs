using CommandLine;
using SavingTime.Bussiness.Commands;
using SavingTime.Data;
using SavingTime.Entities;

namespace SavingTime.Bussiness
{
    [Verb("issue", HelpText = "Register an issue entry.")]
    public class IssueCommand : BaseCommand
    {
        public TimeRecordType TypeRecord { get; set; }

        [Value(0, Required = true, HelpText = "Issue's identifier.")]
        public string? Issue { get; set; }
        private IssueService? issueRecordService { get; set; }

        public IssueCommand()
        {
            TypeRecord = TimeRecordType.Entry;
        }

        public override void Run(SavingTimeDbContext dbContext)
        {
            base.Run(dbContext);
            var now = DateTime.Now;
            var dateTime = new DateTime(
                now.Year,
                now.Month,
                now.Day,
                now.Hour,
                now.Minute,
                0);
            issueRecordService!.Entry(dateTime, Issue);
        }

        protected override void Init()
        {
            issueRecordService = new IssueService(DbContext!);
        }
    }
}
