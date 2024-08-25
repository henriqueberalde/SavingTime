using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SavingTime.Data;
using SavingTime.Entities;

namespace SavingTime.Bussiness.Commands
{
    [Verb("do", HelpText = "Register an entry or exit record, depends on the last command done. If it was an entry then now would be an exit.")]
    public class DoCommand : BaseCommand
    {
        [Value(0, Required = false, HelpText = "Issue to be recorded if it is a entry record")]
        public string? Issue { get; set; }

        public override void Run(IHost host, SavingTimeDbContext dbContext)
        {
            base.Run(host, dbContext);
            var timeRecordService = host.Services.GetService<TimeRecordService>();
            var lastType = timeRecordService!.LastType() ?? TimeRecordType.Exit;
            LogTimeCommand command = host.Services.GetService<EntryCommand>();

            if (lastType == TimeRecordType.Entry)
            {
                command = host.Services.GetService<ExitCommand>();
            }

            command.Issue = Issue;
            command.Run(host, DbContext!);
        }
    }
}
