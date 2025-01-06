using CommandLine;
using Integrations.Jira;
using Integrations.Jira.Classes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SavingTime.Business.Helpers;
using SavingTime.Data;
using SavingTime.Entities;

namespace SavingTime.Business.Commands;

[Verb("jira", HelpText = "Log work into jira")]
public class JiraCommand : AbstractMonthCommand
{
    public override void Run(IHost host, SavingTimeDbContext dbContext) {
        base.Run(host, dbContext);

        var dateRef = DateRef ?? DateTime.Now;
        var refDate = DateTimeHelper.FirstDayOfMonth(dateRef).Date;

        var issues = dbContext.IssueRecords
            .Where(tr => tr.Time >= refDate
                         && tr.Time <= DateTimeHelper.LastTimeOfDay(DateTimeHelper.LastDayOfMonth(refDate))
                         && !tr.Sent)
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
            var items = groupItem.OrderBy(r => r.Time).ThenByDescending(r => r.Id);
            DateTime? currentBeginDateTime = null;
            IssueRecord? entry = null;

            foreach (var record in items)
            {
                if (record.Type == TimeRecordType.Entry)
                {
                    entry = record;
                }
                else
                {
                    if (entry == null)
                        throw new Exception($"Error to process record {record.Time:yyyy/MM/dd} on day {groupItem.Key:yyyy/MM/dd}.");

                    var timeSpanDiff = record.Time - entry.Time;

                    var worklog = new JiraWorklog
                    {
                        Issue = groupItem.Key.Issue,
                        Message = groupItem.Key.Issue,
                        DateTime = record.Time,
                        TimeSpentInSeconds = timeSpanDiff.TotalSeconds,
                        Sent = entry.Sent || record.Sent
                    };

                    sendToJira(worklog);
                    entry.Sent = true;
                    record.Sent = true;
                    dbContext.SaveChanges();
                }
            }

        }
    }

    private void sendToJira(JiraWorklog worklog)
    {
        var prefixIssues = new List<string>{"CNBSE-", "CRD-"};
        var hours = worklog.TimeSpentInSeconds / 60 / 60;
        var str = $"{worklog.DateTime:dd/MM/yyyy} | {worklog.Issue} - {worklog.TimeSpentInSeconds} ({hours:00.00} in hours))";

        if (worklog.Sent)
        {
            Console.WriteLine($"Has been sent before {str}");
            return;
        }

        if (!prefixIssues.Any(p => worklog.Issue.StartsWith(p))) {
            Console.WriteLine($"Ignored              {str}");
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
            Console.WriteLine($"Sent                 {str}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
