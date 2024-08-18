using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SavingTime.Data;
using SavingTime.Entities;

namespace SavingTime.Bussiness.Commands
{
    [Verb("issue", HelpText = "Register an issue entry.")]
    public class IssueCommand : AbstractTimeCommand
    {
        public TimeRecordType TypeRecord { get; set; }

        [Value(0, Required = true, HelpText = "Issue's identifier.")]
        public string? Issue { get; set; }

        public IssueCommand()
        {
            TypeRecord = TimeRecordType.Entry;
        }

        public override void Run(IHost host, SavingTimeDbContext dbContext)
        {
            var issueRecordService = host.Services.GetService<IssueService>();
            base.Run(host, dbContext);
            issueRecordService!.Entry(DateTimeConvertedOrNow(), Issue);
        }
    }
}
