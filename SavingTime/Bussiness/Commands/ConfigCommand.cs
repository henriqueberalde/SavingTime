using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SavingTime.Bussiness.Commands;
using SavingTime.Data;
using System.ComponentModel;

namespace SavingTime.Bussiness
{
    [Verb("config", HelpText = "Show configuration values.")]
    public class ConfigCommand : BaseCommand
    {
        public override void Run(IHost host, SavingTimeDbContext dbContext) {
            var pontoConfig = host.Services.GetService<PontoConfiguration>();
            var jiraConfig = host.Services.GetService<JiraConfiguration>();

            Console.WriteLine("\n");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("PontoConfig");
            Console.ResetColor();
            showProperties(pontoConfig);

            Console.WriteLine("\n");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("JiraConfig");
            Console.ResetColor();
            showProperties(jiraConfig);
        }

        private void showProperties(object obj) {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(obj);
                Console.WriteLine("  {0}: {1}", name, value);
            }
        }
    }
}
