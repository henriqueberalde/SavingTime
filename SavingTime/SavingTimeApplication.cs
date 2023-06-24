using CommandLine;
using Microsoft.EntityFrameworkCore;

namespace SavingTime
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
                Console.Write(">");
                var line = Console.In.ReadLine();

                if (line is null)
                    break;

                var stdin = line.Split(' ');

                Parser.Default.ParseArguments<Options>(stdin)
                .WithParsed(o =>
                {
                    DateTime? datetime = null;

                    if (o.DateTime is not null)
                    {
                        datetime = DateTime.ParseExact(
                            o.DateTime,
                            "yyyy-MM-ddTHH:mm",
                            System.Globalization.CultureInfo.InvariantCulture);
                    } else if (o.Time is not null)
                    {
                        var timeParts = o.Time.Split(':');

                        var isValid =
                            (o.Time.Length == 5) &&
                            o.Time.Contains(":") &&
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
                            throw new ArgumentException($"Time is invalid {o.Time}. Correct format: HH:mm");
                        }

                        var now = DateTime.Now;
                        datetime = new DateTime(
                            DateTime.Now.Year,
                            DateTime.Now.Month,
                            DateTime.Now.Day,
                            hour,
                            minute,
                            0
                        );
                    }

                    if (o.Entry) { Entry(o.Context, datetime); }
                    if (o.Exit) { Exit(o.Context, datetime); }
                    if (o.History) { History(); }
                });
            }
        }

        public void Entry(string? context, DateTime? dateTime)
        {
            RegisterTimeRecord(TimeRecordType.Entry, context, dateTime);
        }

        public void Exit(string? context, DateTime? dateTime)
        {
            RegisterTimeRecord(TimeRecordType.Exit, context, dateTime);
        }

        public void RegisterTimeRecord(TimeRecordType type, string? context, DateTime? dateTime)
        {
            var timeRecord = new TimeRecord(
                dateTime ?? DateTime.Now,
                type
            );
            _dbContext.TimeRecords.Add(timeRecord);
            _dbContext.SaveChanges();
            Console.WriteLine($"{type} Registered");
        }

        public void History()
        {
            var query = _dbContext.TimeRecords.OrderByDescending((t) => t.Time);

            Console.WriteLine("All TimeRecords from data base:\n");
            foreach (var item in query)
            {
                ConsoleColor color;// = ConsoleColor.Red;
                if (item.Type == TimeRecordType.Entry)
                    color = ConsoleColor.Green;
                else
                    color = ConsoleColor.Red;

                Console.ForegroundColor = color;
                Console.WriteLine($"{item.Id} {item.Type.ToString()} - {item.Time}");
                Console.ResetColor();
            }
        }
    }
}
