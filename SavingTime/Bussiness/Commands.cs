using CommandLine;
using SavingTime.Entities;

namespace SavingTime.Bussiness
{
    public abstract class LogTimeCommand
    {
        public TimeRecordType TypeRecord { get; set; }

        [Option('d', "date", Required = false, HelpText = "The Datetime of the record. Format: YYYY-MM-DDTHH:MM")]
        public string? DateTime { get; set; }

        [Option('t', "time", Required = false, HelpText = "The Time of the record. Format: HH:MM")]
        public string? Time { get; set; }

        [Option('c', "context", Required = false, HelpText = "Context of the record.")]
        public string? Context { get; set; }

        [Option("no-integration", HelpText = "Context of the record.")]
        public bool CancelIntegration { get; set; }

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
            return DateTime is not null
                ? System.DateTime.ParseExact(
                    DateTime,
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
    public class DoCommand
    {
    }

    [Verb("info", HelpText = "Show informations about the day.")]
    public class InfoCommand
    {
    }

    [Verb("history", HelpText = "All time records.")]
    public class HistoryCommand
    {
    }

    [Verb("summary", HelpText = "Summary of the entire base.")]
    public class SummaryCommand
    {
    }

    [Verb("test", HelpText = "Test browser integration.")]
    public class TestIntegrationCommand
    {
    }
}
