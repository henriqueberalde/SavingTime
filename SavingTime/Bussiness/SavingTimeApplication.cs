using CommandLine;
using Microsoft.Extensions.Primitives;
using SavingTime.Data;
using SavingTime.Entities;
using System.Collections.Generic;
using System.Text;

namespace SavingTime.Bussiness
{
    public interface ISavingTimeApplication
    {
        void Run();
    }

    public class SavingTimeApplication : ISavingTimeApplication
    {
        private readonly SavingTimeDbContext _dbContext;

        public SavingTimeApplication(SavingTimeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Run()
        {
            while (true)
            {
                // ShowSummary();

                Console.WriteLine("\n>");
                var line = Console.In.ReadLine();

                if (line is null)
                    break;

                var stdin = line.Split(' ');

                Parser.Default.ParseArguments<Options>(stdin)
                .WithParsed(o =>
                {
                    if (o.Summary) { ShowSummary(); }
                    if (o.History) { History(); }
                    if (o.Entry || o.Exit) { RegisterTimeRecord(o); }
                });
            }
        }

        public void RegisterTimeRecord(Options o)
        {
            var timeRecord = new TimeRecord(
                o.DateTimeConverted ?? DateTime.Now,
                o.TypeRecord,
                o.Context
            );
            _dbContext.TimeRecords.Add(timeRecord);
            _dbContext.SaveChanges();
            Console.WriteLine($"{o.TypeRecord} Registered");
        }


        public void History()
        {
            var query = _dbContext.TimeRecords.OrderBy((t) => t.Time);

            Console.WriteLine("All TimeRecords from data base:\n");

            foreach (var item in query)
            {
                ConsoleColor color;
                if (item.Type == TimeRecordType.Entry)
                    color = ConsoleColor.Green;
                else
                    color = ConsoleColor.Red;

                Console.ForegroundColor = color;
                Console.WriteLine(item.ToString());
                Console.ResetColor();
            }
        }

        public void ShowSummary()
        {
            var list = _dbContext.TimeRecords.OrderBy((t) => t.Time).ToList();

            Console.WriteLine("Summary:\n");
            var summary = Summary.FromTimeRecordList(list);

            Console.WriteLine(summary.ToString());
        }
    }
}
