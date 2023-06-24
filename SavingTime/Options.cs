using CommandLine;

namespace SavingTime
{
    public class Options
    {
        [Option('e', "entry", Required = false, HelpText = "Register an entry record.")]
        public bool Entry { get; set; }

        [Option('x', "exit", Required = false, HelpText = "Register an exit record.")]
        public bool Exit { get; set; }

        [Option('d', "date", Required = false, HelpText = "The Datetime of the record. Format: YYYY-MM-DDTHH:MM")]
        public string ?DateTime { get; set; }

        [Option('t', "time", Required = false, HelpText = "The Time of the record. Format: HH:MM")]
        public string ?Time { get; set; }

        [Option('c', "context", Required = false, HelpText = "Context of the record.")]
        public string ?Context { get; set; }

        [Option("history", Required = false, HelpText = "All time records.")]
        public bool History { get; set; }

        public TimeRecordType TypeRecord
        {
            get
            {
                if (Entry) { return TimeRecordType.Entry; }
                if (Exit) { return TimeRecordType.Exit; }

                throw new Exception($"No type record passed. You must choose entry or exit record");
            }
        }

        public DateTime? DateTimeConverted {

            get
            {
                return ConvertDateTime() ?? ConvertTime();
            }
        }

        private DateTime? ConvertDateTime()
        {
            return DateTime is not null
                ? System.DateTime.ParseExact(
                    DateTime,
                    "yyyy-MM-ddTHH:mm",
                    System.Globalization.CultureInfo.InvariantCulture
                )
                : null;
        }

        private DateTime? ConvertTime()
        {
            if (Time is null) { return null; }

            var timeParts = Time.Split(':');

            var isValid =
                (Time.Length == 5) &&
                Time.Contains(':') &&
                (timeParts.Length == 2);

            var isHourNumber = int.TryParse(timeParts[0], out var hour);
            var isMinuteNumber = int.TryParse(timeParts[1], out var minute);

            isValid = isValid &&
                isHourNumber &&
                isMinuteNumber &&
                (hour >= 0 && hour <= 23) &&
                (minute >= 0 && minute <= 59);

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
}
