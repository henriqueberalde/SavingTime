using CommandLine;
using System;

namespace SavingTime
{
    internal class Program  
    {
        static void Main(string[] args)
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
                    TimeSpan? time = null;

                    if (o.Time is not null)
                    {
                        time = TimeSpan.ParseExact(
                            o.Time,
                            "yyyy-MM-dd HH:mm",
                            System.Globalization.CultureInfo.InvariantCulture);
                    }

                    if (o.DateTime is not null)
                    {
                        datetime = DateTime.ParseExact(
                            o.DateTime,
                            "yyyy-MM-dd HH:mm",
                            System.Globalization.CultureInfo.InvariantCulture);
                    }                    

                    if (o.Entry) { Entry(o.Context, datetime, time); }
                    if (o.Exit) { Exit(o.Context, datetime, time); }
                    if (o.History) { History(); }
                });
            }
        }

        public static void Entry(string? context, DateTime? dateTime, TimeSpan? time)
        {
            RegisterTimeRecord(TimeRecordType.Entry);
        }

        public static void Exit(string? context, DateTime? dateTime, TimeSpan? time)
        {
            RegisterTimeRecord(TimeRecordType.Exit);
        }

        public static void RegisterTimeRecord(TimeRecordType type, DateTime? date = null)
        {
            using (var db = new SavingTimeDbContext())
            {
                var dateRecord = DateTime.Now;

                if (date.HasValue)
                    dateRecord = date.Value;
                else
                    dateRecord = DateTime.Now;

                var timeRecord = new TimeRecord(dateRecord, type);
                db.TimeRecords.Add(timeRecord);
                db.SaveChanges();
                Console.WriteLine($"{type.ToString()} Registered");
            }
        }

        public static void History()
        {
            using (var db = new SavingTimeDbContext())
            {
                var query = db.TimeRecords.OrderByDescending((t) => t.Time);

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
}