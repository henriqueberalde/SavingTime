using CommandLine;
using Microsoft.Extensions.Hosting;
using SavingTime.Business.Helpers;
using SavingTime.Data;

namespace SavingTime.Business.Commands
{
    [Verb("issue-summary", HelpText = "Summary of the issue record on the period (default: current month).")]
    public class IssueSummaryCommand : AbstractMonthCommand
    {
        public override void Run(IHost host, SavingTimeDbContext dbContext)
        {
            base.Run(host, dbContext);
            var query = dbContext.IssueRecords.AsQueryable();
            var dateRef = DateRef ?? DateTime.Now;
            var refDate = DateTimeHelper.FirstDayOfMonth(dateRef).Date;
            query = dbContext.IssueRecords.Where(tr => tr.Time >= refDate && tr.Time <= DateTimeHelper.LastTimeOfDay(DateTimeHelper.LastDayOfMonth(refDate)));

            var issues = query
                .OrderBy(t => t.Time)
                .ThenByDescending(t => t.Id)
                .ToList();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nIssue Summary:");
            Console.ResetColor();
            new IssueService(dbContext).Summary(issues);
        }
    }
}
