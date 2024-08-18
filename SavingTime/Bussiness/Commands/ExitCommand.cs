using CommandLine;
using SavingTime.Entities;

namespace SavingTime.Bussiness.Commands
{
    [Verb("exit", HelpText = "Register an exit record.")]
    public class ExitCommand : LogTimeCommand
    {
        public ExitCommand() : base(TimeRecordType.Exit)
        {
        }
    }
}
