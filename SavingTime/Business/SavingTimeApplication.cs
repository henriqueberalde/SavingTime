﻿using CommandLine;
using Microsoft.Extensions.Hosting;
using SavingTime.Business.Commands;
using SavingTime.Data;

namespace SavingTime.Business
{
    public interface ISavingTimeApplication
    {
        void Run(IHost host);
    }

    public class SavingTimeApplication : ISavingTimeApplication
    {
        private readonly SavingTimeDbContext dbContext;

        public SavingTimeApplication(SavingTimeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Run(IHost host)
        {
            Console.WriteLine("SAVING TIME\n");

            try
            {
                RunSummaryCommand(host);
            }
            catch (Exception ex)
            {
                TreatException(ex);
            }
            finally
            {
                dbContext.ChangeTracker.Clear();
            }

            Loop(host);
        }

        private void Loop(IHost host)
        {
            while (true)
            {
                try
                {
                    Console.Write(">");
                    var line = Console.In.ReadLine();

                    if (line is null)
                        break;

                    var stdin = ParseCommandLine(line);

                    _ = Parser.Default.ParseArguments<
                            DoCommand,
                            EntryCommand,
                            ExitCommand,
                            IssueCommand,
                            InfoCommand,
                            SummaryCommand,
                            IssueSummaryCommand,
                            HistoryCommand,
                            TestCommand,
                            JiraCommand,
                            ConfigCommand
                        >(stdin)
                        .WithParsed<BaseCommand>(o => o.Run(host, dbContext));
                }
                catch (Exception ex)
                {
                    TreatException(ex);
                }
                finally
                {
                    dbContext.ChangeTracker.Clear();
                }
            }
        }

        private void RunSummaryCommand(IHost host)
        {
            new SummaryCommand().Run(host,dbContext);
        }

        private static void TreatException(Exception ex)
        {
            Console.WriteLine("Error:");
            Console.WriteLine(ex.Message);
            Console.WriteLine("\n");
            Console.WriteLine("StackTrace:");
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine("\n");
        }

        private static string[] ParseCommandLine(string commandLine)
        {
            var args = new List<string>();
            var currentArg = new System.Text.StringBuilder();
            var inQuotes = false;

            foreach (var c in commandLine)
            {
                switch (c)
                {
                    case '"':
                        inQuotes = !inQuotes;
                        break;
                    case ' ' when !inQuotes:
                    {
                        if (currentArg.Length > 0)
                        {
                            args.Add(currentArg.ToString());
                            currentArg.Clear();
                        }

                        break;
                    }
                    default:
                        currentArg.Append(c);
                        break;
                }
            }

            if (currentArg.Length > 0)
            {
                args.Add(currentArg.ToString());
            }

            return args.ToArray();
        }
    }
}
