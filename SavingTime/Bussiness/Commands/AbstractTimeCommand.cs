using CommandLine;
using Microsoft.Extensions.Hosting;
using SavingTime.Data;

namespace SavingTime.Bussiness.Commands
{
    public abstract class AbstractTimeCommand: BaseCommand
    {
        [Option('d', "date", Required = false, HelpText = "The Datetime of the record. Format: YYYY-MM-DDTHH:MM")]
        public string? RefDateTime { get; set; }

        [Option('t', "time", Required = false, HelpText = "The Time of the record. Format: HH:MM")]
        public string? Time { get; set; }

        public DateTime? DateTimeConverted
        {
            get
            {
                return convertDateTime() ?? convertTime();
            }
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
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                hour,
                minute,
                0
            );
        }

        public override void Run(IHost host, SavingTimeDbContext dbContext)
        {
            base.Run(host, dbContext);
        }
    }
}
