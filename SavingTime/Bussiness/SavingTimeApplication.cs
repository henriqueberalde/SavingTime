using CommandLine;
using Integrations;
using SavingTime.Data;
using SavingTime.Entities;

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
            Console.WriteLine("SAVING TIME\n");

            try
            {
                ShowSummary(new SummaryCommand());
            }
            catch (Exception)
            {
                Console.Write("Error on Summary");
            }

            while (true)
            {
                Console.Write(">");
                var line = Console.In.ReadLine();

                if (line is null)
                    break;

                var stdin = line.Split(' ');

                Parser.Default.ParseArguments<
                    DoCommand,
                    EntryCommand,
                    ExitCommand,
                    InfoCommand,
                    SummaryCommand,
                    HistoryCommand,
                    TestIntegrationCommand
                >(stdin)
                .WithParsed(o =>
                {
                    switch (o)
                    {
                        case DoCommand:
                            DecideCommand();
                            break;
                        case EntryCommand:
                            RegisterTimeRecord((EntryCommand)o);
                            break;
                        case ExitCommand:
                            RegisterTimeRecord((ExitCommand)o);
                            break;
                        case InfoCommand:
                            ShowInfo();
                            break;
                        case SummaryCommand:
                            ShowSummary((SummaryCommand)o);
                            break;
                        case HistoryCommand:
                            History();
                            break;
                        case TestIntegrationCommand:
                            TestBrowserIntegration();
                            break;
                    }
                });
            }
        }

        public void DecideCommand()
        {
            var lastType = LastType() ?? TimeRecordType.Exit;
            LogTimeCommand command = new EntryCommand();

            if (lastType == TimeRecordType.Entry) {
                command = new ExitCommand();
            }

            RegisterTimeRecord(command);
        }

        public void RegisterTimeRecord(LogTimeCommand o)
        {
            var dateTime = o.DateTimeConverted ?? DateTime.Now;
            var timeRecord = new TimeRecord(
                dateTime,
                o.TypeRecord,
                o.Context
            );
            _dbContext.TimeRecords.Add(timeRecord);
            _dbContext.SaveChanges();

            if (!o.CancelIntegration)
            {
                var dateTimePlus10Min = dateTime.AddSeconds(60 * 10);
                var config = new LacunaConfiguration(
                    "carlosb",
                    "Henrique0428!",
                    "Carlos Beralde",
                    dateTime,
                    o.TypeRecord.ToString()
                );
                var interation = new LacunaIntegration(config);

                var todayInitial = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
                var todayEnd = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
                var line = _dbContext.TimeRecords.Where(t => t.Time >= todayInitial && t.Time <= todayEnd && t.Type == o.TypeRecord).Count() - 1;

                interation.Login();
                interation.LogWork(
                    line,
                    dateTime.ToString("HH:mm"),
                    dateTimePlus10Min.ToString("HH:mm"));
            }

            Console.WriteLine($"{o.TypeRecord} Registered");
            ShowInfo();
        }

        public void TestBrowserIntegration()
        {
            Console.WriteLine("Testing Browser integration\n");

            var config = new LacunaConfiguration(
                "carlosb",
                "Henrique0428!",
                "Carlos Beralde",
                DateTime.Now,
                ""
            );
            var interation = new LacunaIntegration(config);
            try
            {
                interation.Test();
                Console.WriteLine("Browser integration test sucessed");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Browser integration test FAILED");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void History()
        {
            var list = _dbContext.TimeRecords.OrderBy((t) => t.Time);
            Console.WriteLine("All TimeRecords from data base:\n");
            ShowTimeRecorList(list);
        }

        public void ShowSummary(SummaryCommand o)
        {
            var now = DateTime.Now;
            var list = _dbContext
                .TimeRecords
                .OrderBy((t) => t.Time)
                .ToList();

            if (LastType() == TimeRecordType.Entry) {
                list.Add(new TimeRecord(DateTime.Now, TimeRecordType.Exit, null));
            }

            Console.WriteLine("Summary:\n");
            var summary = Summary.FromTimeRecordList(list);
            summary.Number = o.ParsedNumber();
            Console.WriteLine(summary.ToString());
        }

        public void ShowInfo() {
            var now = DateTime.Now;
            var list = _dbContext.TimeRecords
                .Where(t =>
                    t.Time.Year == now.Year &&
                    t.Time.Month == now.Month &&
                    t.Time.Day == now.Day
                ).OrderBy((t) => t.Time);
            ShowTimeRecorList(list);
        }

        public void ShowTimeRecorList(IEnumerable<TimeRecord> list)
        {
            foreach (var time in list)
            {
                ConsoleColor color;
                if (time.Type == TimeRecordType.Entry)
                    color = ConsoleColor.Green;
                else
                    color = ConsoleColor.Red;

                Console.ForegroundColor = color;
                Console.WriteLine(time.ToString());
                Console.ResetColor();
            }
        }

        private TimeRecordType? LastType() {
            return _dbContext.TimeRecords.OrderByDescending(r => r.Time).FirstOrDefault()?.Type;
        }
    }
}
