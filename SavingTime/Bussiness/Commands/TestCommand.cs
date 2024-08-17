using CommandLine;
using Integrations;
using Integrations.Ponto;
using Integrations.Ponto.Classes;
using Microsoft.Extensions.Hosting;
using SavingTime.Bussiness.Commands;
using SavingTime.Data;

namespace SavingTime.Bussiness
{
    [Verb("test", HelpText = "Test integrations.")]
    public class TestCommand : BaseCommand
    {
        [Option('t', "type", Required = true, HelpText = "Type of test to be made (browser, jira)")]
        public string Type { get; set; }

        public override void Run(IHost host, SavingTimeDbContext dbContext) {
            base.Run(host, dbContext);
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

            var options = new PontoIntegrationOptions(
                "carlosb",
                "Henrique0428!",
                "Carlos Beralde",
                DateTime.Now,
                ""
            );
            var interation = new PontoIntegration(options);
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
