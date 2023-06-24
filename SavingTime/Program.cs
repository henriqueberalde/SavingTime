using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SavingTime
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IHost _host = Host.CreateDefaultBuilder().ConfigureServices(services => {
                
                services.AddSingleton<ISavingTimeApplication, SavingTimeApplication>();
                services.AddSingleton<SavingTimeDbContext, SavingTimeDbContext>();
                services.AddSingleton<DbContext, SavingTimeDbContext>();

            }).Build();

            var app = _host.Services.GetRequiredService<ISavingTimeApplication>();
            app.Run();
        }
    }
}
