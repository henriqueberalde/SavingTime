﻿using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SavingTime.Business.Helpers;
using SavingTime.Data;
using SavingTime.Entities;

namespace SavingTime.Business.Commands;

[Verb("summary", HelpText = "Summary of the time records on the period (default: current month).")]
public class SummaryCommand : AbstractMonthCommand
{
    public override void Run(IHost host, SavingTimeDbContext dbContext)
    {
        base.Run(host, dbContext);
        var timeRecordService = host.Services.GetService<TimeRecordService>();

        IQueryable<TimeRecord> query;
        var dateRef = DateRef ?? DateTime.Now;
        var refDate = DateTimeHelper.FirstDayOfMonth(dateRef).Date;

        query = DbContext!.TimeRecords.Where(tr => tr.Time >= refDate && tr.Time <= DateTimeHelper.LastTimeOfDay(DateTimeHelper.LastDayOfMonth(refDate)));

        var list = query
            .OrderBy(t => t.Time)
            .ThenByDescending(t => t.Id)
            .ToList();

        if (timeRecordService!.LastType(list) == TimeRecordType.Entry)
        {
            list.Add(new TimeRecord(dateRef, TimeRecordType.Exit));
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\nSummary:");
        Console.ResetColor();
        var summary = Summary.FromTimeRecordList(list);
        Console.WriteLine(summary.ToString());
    }
}
