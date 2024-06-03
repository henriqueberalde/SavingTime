using CommandLine;
using SavingTime.Bussiness.Commands;
using SavingTime.Bussiness.Helpers;
using SavingTime.Data;
using SavingTime.Entities;

namespace SavingTime.Bussiness
{
    [Verb("summary", HelpText = "Summary of the time records on the period (default: current month).")]
    public class SummaryCommand : BaseCommand
    {
        [Option('m', "month", Required = false, HelpText = "Month of records to show")]
        public int? Month { get; set; }


        private TimeRecordService? timeRecordService { get; set; }

        private IssueService? issueRecordService { get; set; }

        public override void Run(SavingTimeDbContext _)
        {
            base.Run(_);
            try
            {
                showSummary();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error on Summary");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        protected override void Init()
        {
            issueRecordService = new IssueService(DbContext!);
            timeRecordService = new TimeRecordService(DbContext!, issueRecordService);
        }

        private void showSummary() {
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
