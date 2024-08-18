using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SavingTime.Bussiness.Helpers;
using SavingTime.Data;
using SavingTime.Entities;

namespace SavingTime.Bussiness.Commands
{
    [Verb("summary", HelpText = "Summary of the time records on the period (default: current month).")]
    public class SummaryCommand : BaseCommand
    {
        [Option('m', "month", Required = false, HelpText = "Month of records to show")]
        public int? Month { get; set; }

        public override void Run(IHost host, SavingTimeDbContext dbContext)
        {
            base.Run(host, dbContext);
            var timeRecordService = host.Services.GetService<TimeRecordService>();

            IQueryable<TimeRecord> query;
            var dateRef = Month.HasValue ? new DateTime(DateTime.Now.Year, Month.Value, 1) : DateTime.Now;
            var refDate = DateTimeHelper.FirstDayOfMonth(dateRef).Date;

            query = DbContext!.TimeRecords.Where(tr => tr.Time >= refDate && tr.Time <= DateTimeHelper.LastTimeOfDay(DateTimeHelper.LastDayOfMonth(refDate)));

            var list = query
                .OrderBy(t => t.Time)
                .ThenByDescending(t => t.Id)
                .ToList();

            if (timeRecordService!.LastType(list) == TimeRecordType.Entry)
            {
                list.Add(new TimeRecord(dateRef, TimeRecordType.Exit, null));
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nSummary:");
            Console.ResetColor();
            var summary = Summary.FromTimeRecordList(list);
            Console.WriteLine(summary.ToString());
        }
    }
}
