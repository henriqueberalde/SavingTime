using CommandLine;

namespace SavingTime
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var line = Console.In.ReadLine();

                if (line is null)
                    break;

                var stdin = line.Split(' ');

                Parser.Default.ParseArguments<Options>(stdin)
                .WithParsed<Options>(o =>
                {
                    if (o.Verbose)
                    {
                        Console.WriteLine($"VERBOSE");
                    }
                    else if (o.Help)
                    {
                        Console.WriteLine($"HELP");
                    }
                    else
                    {
                        Console.WriteLine($"NOTHING");
                    }
                });

                /*Console.Write("Enter a command >");
                var command = Console.ReadLine();

                command = command is not null ? command.Trim() : "";

                if (string.IsNullOrEmpty(command))
                    continue;

                try
                {
                    switch (command)
                    {
                        case "entry":
                            throw new NotImplementedException();
                        // break;
                        case "exit":
                            throw new NotImplementedException();
                        // break;
                        case "history":
                            throw new NotImplementedException();
                        // break;
                        case "help":
                            throw new NotImplementedException();
                        // break;
                        default:
                            // throw new CommandInvalidException();
                            throw new Exception();
                    }
                }
                catch (CommandInvalidException)
                {
                    Console.WriteLine("Invalid Command");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error executing action");
                    Console.WriteLine(ex);
                }

                Console.WriteLine();
                Console.WriteLine();*/
            }
        }

        public static void Help()
        {
            Console.WriteLine("ALL COMMANDS\n");
            Console.WriteLine($"entry  : Save an entry record");
            Console.WriteLine($"entry  : Save an exit record");
            Console.WriteLine($"history: Show history of records");
            Console.WriteLine($"help   : Show this message");
        }

       /* public static void Entry()
        {
            RegisterTimeRecord(TimeRecordType.Entry);
        }

        public static void Exit()
        {
            RegisterTimeRecord(TimeRecordType.Exit);
        }

        public static void RegisterTimeRecord(TimeRecordType type, DateTime? date = null)
        {
            using (var db = new TimeRecordContext())
            {
                var dateRecord = DateTime.Now;

                if (date.HasValue)
                    dateRecord = date.Value;
                else
                    dateRecord = DateTime.Now;

                var timeRecord = new TimeRecord(dateRecord, type);
                db.TimeRecords.Add(timeRecord);
                db.SaveChanges();
                Console.Write($"{type.ToString()} Registered");
            }
        }*/
    }
}