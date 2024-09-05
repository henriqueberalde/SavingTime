using CommandLine;
using SavingTime.Entities;

namespace SavingTime.Business.Commands
{
    [Verb("entry", HelpText = "Register an entry record.")]
    public class EntryCommand : LogTimeCommand
    {
        public EntryCommand() : base(TimeRecordType.Entry)
        {
        }
    }
}
