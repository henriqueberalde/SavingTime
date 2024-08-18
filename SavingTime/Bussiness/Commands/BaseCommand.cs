using Microsoft.Extensions.Hosting;
using SavingTime.Data;

namespace SavingTime.Bussiness.Commands
{
    public abstract class BaseCommand
    {
        protected SavingTimeDbContext? DbContext { get; set; }
        protected IHost Host { get; set; }

        public virtual void Run(IHost host, SavingTimeDbContext dbContext) {
            DbContext = dbContext;
            Host = host;
        }
    }
}
