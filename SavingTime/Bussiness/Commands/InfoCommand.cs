using CommandLine;
using SavingTime.Bussiness.Commands;
using SavingTime.Bussiness.Helpers;
using SavingTime.Data;
using SavingTime.Entities;
using System.Collections.Generic;

namespace SavingTime.Bussiness
{
    [Verb("info", HelpText = "Show informations about the day.")]
    public class InfoCommand : BaseCommand
    {
        [Option('d', "date", Required = false, HelpText = "The date of the record to show. Format: YYYY-MM-DD")]
        public string? RefDateTime { get; set; }

        public DateTime? DateTimeConverted
        {
            get
            {
                return convertDateTime();
            }
        }

        private DateTime? convertDateTime()
        {
            return RefDateTime is not null
                ? System.DateTime.ParseExact(
                    RefDateTime,
                    "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture
                )
                : null;
        }

        public override void Run(SavingTimeDbContext dbContext)
        {
            var refDate = DateTimeConverted ?? DateTime.Now;

            var timeList = dbContext.TimeRecords
                .Where(t =>
                    t.Time.Year == refDate.Year &&
                    t.Time.Month == refDate.Month &&
                    t.Time.Day == refDate.Day
                )
                .OrderBy(t => t.Time)
            .ThenBy(t => t.Id);

            var issueList = dbContext.IssueRecords
                .Where(t =>
                    t.Time.Year == refDate.Year &&
                    t.Time.Month == refDate.Month &&
                    t.Time.Day == refDate.Day
                )
                .OrderBy(t => t.Time)
                .ThenBy(t => t.Id);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nTime Records:");
            Console.ResetColor();
            ConsoleWriterHelper.ShowRecorList(timeList);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nIssues:");
            Console.ResetColor();
            ConsoleWriterHelper.ShowRecorList(issueList);
        }
    }
}
