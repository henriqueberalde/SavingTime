using SavingTime.Data;

namespace SavingTime.Bussiness
{
    public class TimeService
    {
        private readonly SavingTimeDbContext dbContext;

        public TimeService(SavingTimeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool IsWorking()
        {
            return dbContext.TimeRecords.OrderByDescending(t => t.Time).First().Type == Entities.TimeRecordType.Entry;
        }
    }
}
