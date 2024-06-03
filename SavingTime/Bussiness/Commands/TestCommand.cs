using CommandLine;
using Integrations;
using SavingTime.Bussiness.Commands;
using SavingTime.Data;

namespace SavingTime.Bussiness
{
    [Verb("test", HelpText = "Test integrations.")]
    public class TestCommand : BaseCommand
    {
        [Option('t', "type", Required = true, HelpText = "Type of test to be made (browser, jira)")]
        public string Type { get; set; }

        public override void Run(SavingTimeDbContext dbContext) {
            base.Run(dbContext);
            switch (Type)
            {
                case "browser":
                    testBrowserIntegration();
                    break;
                case "jira":
                    Console.WriteLine("Not implemented");
                    break;
            }
        }

        private void testBrowserIntegration()
        {
            Console.WriteLine("Testing Browser integration\n");

            var config = new LacunaConfiguration(
                "carlosb",
                "<password>",
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
    }
}
