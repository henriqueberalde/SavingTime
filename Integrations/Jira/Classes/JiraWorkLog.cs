namespace Integrations.Jira.Classes {
    public class JiraWorklog
    {
        public string Issue { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
        public bool Sent { get; set; }

        public string DateTimeFormated
        {
            get { return DateTime.ToString("yyyy-MM-dd'T'HH:mm:ss.fff-0300"); }
        }
        public double TimeSpentInSeconds { get; set; }
    }
}