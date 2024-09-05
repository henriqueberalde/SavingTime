using CommandLine;
using Microsoft.Extensions.Hosting;
using SavingTime.Business.Helpers;
using SavingTime.Data;

namespace SavingTime.Business.Commands
{
    [Verb("history", HelpText = "All time records and issue records.")]
    public class HistoryCommand : BaseCommand
    {
        [Option('t', "time", Required = false, Default = false, HelpText = "List time records")]
        public bool Time { get; set; }

        [Option('i', "issue", Required = false, Default = false, HelpText = "List issue records")]
        public bool Issue { get; set; }

        [Option('n', "number", Required = false, Default = 15, HelpText = "Number of records to be listed")]
        public int Number { get; set; }

        public override void Run(IHost host, SavingTimeDbContext dbContext)
        {
            if (!Issue && !Time) {
                Issue = true;
                Time = true;
            }

            if (Time) {
                var timeRecords = dbContext.TimeRecords
                .OrderByDescending((t) => t.Time)
                .OrderByDescending(t => t.Id)
                .Take(Number)
                .Reverse();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nTimeRecords:");
                Console.ResetColor();
                ConsoleWriterHelper.ShowRecorList(timeRecords);
            }

            if (Issue) {
                var issueRecords = dbContext.IssueRecords
                .OrderByDescending((t) => t.Time)
                .ThenByDescending(t => t.Id)
                .Take(Number)
                .Reverse();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nIssueRecords:");
                Console.ResetColor();
                ConsoleWriterHelper.ShowRecorList(issueRecords);
            }
        }
    }
}
