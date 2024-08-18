﻿using CommandLine;
using Microsoft.Extensions.Hosting;
using SavingTime.Bussiness.Helpers;
using SavingTime.Data;

namespace SavingTime.Bussiness.Commands
{
    [Verb("history", HelpText = "All time records.")]
    public class HistoryCommand : BaseCommand
    {
        public override void Run(IHost host, SavingTimeDbContext dbContext)
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
