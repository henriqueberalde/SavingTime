using CommandLine;
using Integrations;
using SavingTime.Bussiness.Helpers;
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
        private readonly SavingTimeDbContext dbContext;
        private readonly TimeService timeService;
        private readonly IssueService issueService;

        public SavingTimeApplication(
            SavingTimeDbContext dbContext,
            IssueService issueService,
            TimeService timeService)
        {
            this.dbContext = dbContext;
            this.issueService = issueService;
            this.timeService = timeService;
        }
        public void Run()
        {
            Console.WriteLine("SAVING TIME\n");

            try
            {
                ShowSummary(new SummaryCommand());
            }
            catch (Exception ex)
            {
                Console.Write("Error on Summary");
                Console.Write(ex.Message);
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
                    IssueCommand,
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
                            DecideCommand((DoCommand)o);
                            break;
                        case IssueCommand:
                            RegisterIssueRecord((IssueCommand)o);
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

        public void DecideCommand(DoCommand o)
        {
            var lastType = LastType() ?? TimeRecordType.Exit;
            LogTimeCommand command = new EntryCommand();

            if (lastType == TimeRecordType.Entry) {
                command = new ExitCommand();
            }

            command.Context = o.Context;
            RegisterTimeRecord(command);
        }

        public void RegisterIssueRecord(IssueCommand o)
        {
            var now = DateTime.Now;
            var dateTime = new DateTime(
                now.Year,
                now.Month,
                now.Day,
                now.Hour,
                now.Minute,
                0);
            issueService.Entry(dateTime, o.Issue);
        }

        public void RegisterTimeRecord(LogTimeCommand o)
        {
            var now = DateTime.Now;
            var dateTime = o.DateTimeConverted ?? new DateTime(
                now.Year,
                now.Month,
                now.Day,
                now.Hour,
                now.Minute,
                0);
            var timeRecord = new TimeRecord(
                dateTime,
                o.TypeRecord,
                o.Context
            );

            timeService.Add(timeRecord);

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
                var line = dbContext.TimeRecords.Where(t => t.Time >= todayInitial && t.Time <= todayEnd && t.Type == o.TypeRecord).Count() - 1;

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
            var list = dbContext.TimeRecords
                .OrderBy((t) => t.Time)
                .ThenByDescending(t => t.Id);
            Console.WriteLine("All TimeRecords from data base:\n");
            ShowTimeRecorList(list);
        }

        public void ShowSummary(SummaryCommand o)
        {
            var number = o.ParsedNumber();
            var query = dbContext.TimeRecords.AsQueryable();

            if (number.HasValue)
            {
                var alldays = dbContext.TimeRecords
                    .GroupBy(tr => tr.Time.Date)
                    .Select(r => r.Key)
                    .ToList();
                alldays.Sort();
                alldays.TakeLast(number.Value);

                var filterDate = DateTime.Now.AddDays((number.Value - 1) * -1).Date;
                query = dbContext.TimeRecords.Where(tr => tr.Time >= filterDate);
            }
            else {
                var refDate = DateTimeHelper.FirstDayOfMonth(DateTime.Now).Date;
                query = dbContext.TimeRecords.Where(tr => tr.Time >= refDate);
            }

            var list = query
                .OrderBy(t => t.Time)
                .ThenByDescending(t => t.Id)
                .ToList();

            if (LastType() == TimeRecordType.Entry) {
                list.Add(new TimeRecord(DateTime.Now, TimeRecordType.Exit, null));
            }

            Console.WriteLine("Summary:\n");
            var summary = Summary.FromTimeRecordList(list);
            Console.WriteLine(summary.ToString());

            var issues = dbContext.IssueRecords
                .OrderBy(t => t.Time)
                .ThenByDescending(t => t.Id)
                .ToList();

            new IssueService(dbContext).Summary(issues);
        }

        public void ShowInfo() {
            var now = DateTime.Now;
            var list = dbContext.TimeRecords
                .Where(t =>
                    t.Time.Year == now.Year &&
                    t.Time.Month == now.Month &&
                    t.Time.Day == now.Day
                ).OrderBy(t => t.Time)
                .ThenByDescending(t => t.Id);
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
            return dbContext.TimeRecords
                .OrderByDescending(r => r.Time)
                .ThenByDescending(t => t.Id)
                .FirstOrDefault()?.Type;
        }
    }
}
