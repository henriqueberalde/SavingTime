using SavingTime.Data;

namespace SavingTime.Bussiness.Commands
{
    public abstract class BaseCommand
    {
        protected SavingTimeDbContext? DbContext { get; set; }

        public virtual void Run(SavingTimeDbContext dbContext) {
            DbContext = dbContext;
            Init();
        }
        protected virtual void Init() { }
    }
}
