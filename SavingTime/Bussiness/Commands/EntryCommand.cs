using CommandLine;
using SavingTime.Entities;

namespace SavingTime.Bussiness.Commands
{
    [Verb("entry", HelpText = "Register an entry record.")]
    public class EntryCommand : LogTimeCommand
    {
        public EntryCommand() : base(TimeRecordType.Entry)
        {
        }
    }
}
