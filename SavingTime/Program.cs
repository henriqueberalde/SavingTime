using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SavingTime.Bussiness;
using SavingTime.Bussiness.Commands;
using SavingTime.Data;
using System.Globalization;

namespace SavingTime
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .Build();

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
            IHost _host = Host.CreateDefaultBuilder().ConfigureServices(services => {
                
                services.AddScoped<ISavingTimeApplication, SavingTimeApplication>();
                services.AddSingleton<SavingTimeDbContext, SavingTimeDbContext>();
                services.AddSingleton<DbContext, SavingTimeDbContext>();
                services.AddSingleton<DoCommand>();

                services.AddScoped<TimeRecordService>();
                services.AddScoped<IssueService>();

                var pontoConfig = new PontoConfiguration();
                configuration.Bind("Ponto", pontoConfig);
                services.AddSingleton(pontoConfig);

                var jiraConfig = new JiraConfiguration();
                configuration.Bind("Jira", jiraConfig);
                services.AddSingleton(jiraConfig);
            }).Build();

            var app = _host.Services.GetRequiredService<ISavingTimeApplication>();
            app.Run(_host);
        }
    }
}
