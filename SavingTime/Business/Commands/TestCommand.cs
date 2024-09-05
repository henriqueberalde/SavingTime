using CommandLine;
using Integrations;
using Integrations.Ponto;
using Integrations.Ponto.Classes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SavingTime.Business.Commands;
using SavingTime.Data;

namespace SavingTime.Business
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
                    var pontoConfig = host.Services.GetService<PontoConfiguration>();
                    testBrowserIntegration(new PontoIntegrationOptions(
                            pontoConfig.UserName,
                            pontoConfig.Password,
                            pontoConfig.ExpectedProfileName,
                            DateTime.Now,
                            ""
                        ));
                    break;
                case "jira":
                    Console.WriteLine("Not implemented");
                    break;
            }
        }

        private void testBrowserIntegration(PontoIntegrationOptions pontoOptions)
        {
            Console.WriteLine("Testing Browser integration\n");
            var interation = new PontoIntegration(pontoOptions);
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
