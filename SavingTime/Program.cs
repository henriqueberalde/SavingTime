using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SavingTime.Bussiness;
using SavingTime.Data;

namespace SavingTime
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IHost _host = Host.CreateDefaultBuilder().ConfigureServices(services => {
                
                services.AddScoped<ISavingTimeApplication, SavingTimeApplication>();
                services.AddSingleton<SavingTimeDbContext, SavingTimeDbContext>();
                services.AddSingleton<DbContext, SavingTimeDbContext>();

                services.AddScoped<TimeService>();
                services.AddScoped<IssueService>();

            }).Build();

            var app = _host.Services.GetRequiredService<ISavingTimeApplication>();
            app.Run();
        }
    }
}
