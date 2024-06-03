using CommandLine;
using SavingTime.Bussiness.Commands;
using SavingTime.Bussiness.Helpers;
using SavingTime.Data;
using SavingTime.Entities;

namespace SavingTime.Bussiness
{
    [Verb("history", HelpText = "All time records.")]
    public class HistoryCommand : BaseCommand
    {
        public override void Run(SavingTimeDbContext dbContext)
        {
            var list = dbContext.TimeRecords
                .OrderBy((t) => t.Time)
                .ThenByDescending(t => t.Id);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nAll TimeRecords:\n");
            Console.ResetColor();
            ConsoleWriterHelper.ShowRecorList(list);
        }
    }
}
