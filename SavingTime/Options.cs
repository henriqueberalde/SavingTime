using CommandLine;

namespace SavingTime
{
    public class Options
    {
        [Option('e', "entry", Required = false, HelpText = "Register an entry record.")]
        public bool Entry { get; set; }

        [Option('x', "exit", Required = false, HelpText = "Register an exit record.")]
        public bool Exit { get; set; }

        [Option('d', "date", Required = false, HelpText = "The Datetime of the record. Format: YYYY-MM-DD HH:MM")]
        public string ?DateTime { get; set; }

        [Option('t', "time", Required = false, HelpText = "The Time of the record. Format: HH:MM")]
        public string ?Time { get; set; }

        [Option('c', "context", Required = false, HelpText = "Context of the record.")]
        public string ?Context { get; set; }

        [Option("history", Required = false, HelpText = "All time records.")]
        public bool History { get; set; }
    }
}
