using CommandLine;
using SavingTime.Bussiness.Commands;
using SavingTime.Data;

namespace SavingTime.Bussiness
{
    public interface ISavingTimeApplication
    {
        void Run();
    }

    public class SavingTimeApplication : ISavingTimeApplication
    {
        private readonly SavingTimeDbContext dbContext;

        public SavingTimeApplication(
            SavingTimeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Run()
        {
            Console.WriteLine("SAVING TIME\n");

            RunSummaryCommand();

            while (true)
            {
                Console.Write(">");
                var line = Console.In.ReadLine();

                if (line is null)
                    break;

                var stdin = line.Split(' ');

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
                    JiraCommand
                >(stdin)
                .WithParsed<BaseCommand>(o => o.Run(dbContext));
                
                dbContext.ChangeTracker.Clear();
            }
        }

        private void RunSummaryCommand()
        {
            new SummaryCommand().Run(dbContext);
        }
    }
}
