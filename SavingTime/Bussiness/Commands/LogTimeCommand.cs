using CommandLine;
using Integrations;
using SavingTime.Bussiness.Commands;
using SavingTime.Data;
using SavingTime.Entities;

namespace SavingTime.Bussiness
{
    public abstract class LogTimeCommand: BaseCommand
    {
        public TimeRecordType TypeRecord { get; set; }

        [Option('d', "date", Required = false, HelpText = "The Datetime of the record. Format: YYYY-MM-DDTHH:MM")]
        public string? RefDateTime { get; set; }

        [Option('t', "time", Required = false, HelpText = "The Time of the record. Format: HH:MM")]
        public string? Time { get; set; }

        [Option('c', "context", Required = false, HelpText = "Context of the record.")]
        public string? Context { get; set; }

        [Option("no-integration", HelpText = "Context of the record.")]
        public bool CancelIntegration { get; set; }

        private TimeRecordService? timeRecordService { get; set; }
        private IssueService? issueRecordService { get; set; }

        public DateTime? DateTimeConverted
        {
            get
            {
                return convertDateTime() ?? convertTime();
            }
        }

        public LogTimeCommand(TimeRecordType type)
        {
            TypeRecord = type;
        }

        private DateTime? convertDateTime()
        {
            return RefDateTime is not null
                ? System.DateTime.ParseExact(
                    RefDateTime,
                    "yyyy-MM-ddTHH:mm",
                    System.Globalization.CultureInfo.InvariantCulture
                )
                : null;
        }

        private DateTime? convertTime()
        {
            if (Time is null) { return null; }

            var timeParts = Time.Split(':');

            var isValid =
                Time.Length == 5 &&
                Time.Contains(':') &&
                timeParts.Length == 2;

            var isHourNumber = int.TryParse(timeParts[0], out var hour);
            var isMinuteNumber = int.TryParse(timeParts[1], out var minute);

            isValid = isValid &&
                isHourNumber &&
                isMinuteNumber &&
                hour >= 0 && hour <= 23 &&
                minute >= 0 && minute <= 59;

            if (!isValid)
            {
                throw new ArgumentException($"Time is invalid {Time}. Correct format: HH:mm");
            }

            return new DateTime(
                System.DateTime.Now.Year,
                System.DateTime.Now.Month,
                System.DateTime.Now.Day,
                hour,
                minute,
                0
            );
        }

        public override void Run(SavingTimeDbContext _)
        {
            base.Run(_);
            RegisterTimeRecord();
        }

        protected override void Init()
        {
            issueRecordService = new IssueService(DbContext!);
            timeRecordService = new TimeRecordService(DbContext!, issueRecordService);
        }

        public void RegisterTimeRecord()
        {
            var now = DateTime.Now;
            var dateTime = DateTimeConverted ?? new DateTime(
                now.Year,
                now.Month,
                now.Day,
                now.Hour,
                now.Minute,
                0);
            var timeRecord = new TimeRecord(
                dateTime,
                TypeRecord,
                Context
            );

            timeRecordService!.Add(timeRecord);

            if (!CancelIntegration)
            {
                var dateTimePlus10Min = dateTime.AddSeconds(60 * 10);
                var config = new LacunaConfiguration(
                    "carlosb",
                    "<password>",
                    "Carlos Beralde",
                    dateTime,
                    TypeRecord.ToString()
                );
                var interation = new LacunaIntegration(config);

                var todayInitial = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
                var todayEnd = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
                var line = DbContext!.TimeRecords.Where(t => t.Time >= todayInitial && t.Time <= todayEnd && t.Type == TypeRecord).Count() - 1;

                interation.Login();
                interation.LogWork(
                    line,
                    dateTime.ToString("HH:mm"),
                    dateTimePlus10Min.ToString("HH:mm"));
            }

            Console.WriteLine($"{TypeRecord} Registered");
            RunInfoCommand();
        }

        private void RunInfoCommand()
        {
            new InfoCommand().Run(DbContext!);
        }
    }

    [Verb("entry", HelpText = "Register an entry record.")]
    public class EntryCommand : LogTimeCommand
    {
        public EntryCommand() : base(TimeRecordType.Entry)
        {
        }
    }

    [Verb("exit", HelpText = "Register an exit record.")]
    public class ExitCommand : LogTimeCommand
    {
        public ExitCommand() : base(TimeRecordType.Exit)
        {
        }
    }

    [Verb("do", HelpText = "Register an entry or exit record, depends on the last command done. If it was an entry then now would be an exit.")]
    public class DoCommand: BaseCommand
    {
        [Value(0, Required = false, HelpText = "Context")]
        public string? Context { get; set; }

        private TimeRecordService? timeRecordService { get; set; }
        private IssueService? issueRecordService { get; set; }

        public override void Run(SavingTimeDbContext _)
        {
            base.Run(_);
            var lastType = timeRecordService!.LastType() ?? TimeRecordType.Exit;
            LogTimeCommand command = new EntryCommand();

            if (lastType == TimeRecordType.Entry)
            {
                command = new ExitCommand();
            }

            command.Context = Context;
            command.RegisterTimeRecord();
        }

        protected override void Init()
        {
            issueRecordService = new IssueService(DbContext!);
            timeRecordService = new TimeRecordService(DbContext!, issueRecordService);
        }
    }
}
