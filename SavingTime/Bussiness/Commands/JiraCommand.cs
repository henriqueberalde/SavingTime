using CommandLine;
using Integrations.Jira;
using Integrations.Jira.Classes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SavingTime.Bussiness.Helpers;
using SavingTime.Data;
using SavingTime.Entities;

namespace SavingTime.Bussiness.Commands
{
    [Verb("jira", HelpText = "Log work into jira")]
    public class JiraCommand : BaseCommand
    {
        [Option('m', "month", Required = false, HelpText = "Month of records to log into jira")]
        public int? Month { get; set; }

        public override void Run(IHost host, SavingTimeDbContext dbContext) {
            base.Run(host, dbContext);
            
            var dateRef = Month.HasValue ? new DateTime(DateTime.Now.Year, Month.Value, 1) : DateTime.Now;
            var refDate = DateTimeHelper.FirstDayOfMonth(dateRef).Date;

            var issues = dbContext.IssueRecords
                .Where(tr => tr.Time >= refDate && tr.Time <= DateTimeHelper.LastTimeOfDay(DateTimeHelper.LastDayOfMonth(refDate)))
                .OrderBy(t => t.Time)
                .ThenByDescending(t => t.Id)
                .ToList();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nSending Issues to Jira:");
            Console.ResetColor();

            if (!issues.Any())
            {
                Console.WriteLine("\nNo records to send to jira");
                return;
            }

            foreach (var groupItem in issues.GroupBy(r => new { r.Time.Date, r.Issue }))
            {
                var slices = new List<TimeSpan>();
                var items = groupItem.OrderBy(r => r.Time).ThenByDescending(r => r.Id);
                DateTime? currentBeginDateTime = null;

                foreach (var record in items)
                {
                    if (record.Type == TimeRecordType.Entry)
                    {
                        currentBeginDateTime = record.Time;
                    }
                    else
                    {
                        if (!currentBeginDateTime.HasValue)
                            throw new Exception($"Error to process record {record.Time:yyyy/MM/dd} on day {groupItem.Key:yyyy/MM/dd}.");

                        var timeSpanDiff = record.Time - currentBeginDateTime.Value;

                        var worklog = new JiraWorklog
                        {
                            Issue = groupItem.Key.Issue,
                            Message = groupItem.Key.Issue,
                            DateTime = record.Time,
                            TimeSpentInSeconds = timeSpanDiff.TotalSeconds
                        };

                        sendToJira(worklog);
                    }
                }
                
            }
        }

        private void sendToJira(JiraWorklog worklog) {
            var hours = worklog.TimeSpentInSeconds / 60 / 60;
            var str = $"{worklog.DateTime:dd/MM/yyyy} | {worklog.Issue} - {worklog.TimeSpentInSeconds} ({hours:00.00} in hours))";

            if (!worklog.Issue.StartsWith("CNBSE-")) {
                Console.WriteLine($"Ignored {str}");
                return;
            }

            try
            {
                var jiraConfig = Host.Services.GetService<JiraConfiguration>();
                var option = new JiraIntegrationOptions {
                    Url = jiraConfig.Url,
                    Endpoint = jiraConfig.Endpoint,
                    Token = jiraConfig.Token,
                };

                var jiraIntegration = new JiraIntegration(option);
                jiraIntegration.PostWorklog(worklog).Wait();
                Console.WriteLine($"Sent {str}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
