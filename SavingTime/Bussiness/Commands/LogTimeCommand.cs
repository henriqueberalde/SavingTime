using CommandLine;
using Integrations.Ponto;
using Integrations.Ponto.Classes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SavingTime.Data;
using SavingTime.Entities;
using System;

namespace SavingTime.Bussiness.Commands
{
    public abstract class LogTimeCommand: AbstractTimeCommand
    {
        public TimeRecordType TypeRecord { get; set; }

        [Option('c', "context", Required = false, HelpText = "Context of the record. If context is passed an issue will be record with it's value")]
        public string? Context { get; set; }

        [Option("no-integration", HelpText = "Context of the record.")]
        public bool CancelIntegration { get; set; }

        public LogTimeCommand(TimeRecordType type)
        {
            TypeRecord = type;
        }

        public override void Run(IHost host, SavingTimeDbContext dbContext)
        {
            base.Run(host, dbContext);

            var pontoConfig = host.Services.GetService<PontoConfiguration>();
            var timeRecordService = host.Services.GetService<TimeRecordService>();
            var dateTime = DateTimeConvertedOrNow();
            var timeRecord = new TimeRecord(
                dateTime,
                TypeRecord,
                Context
            );

            timeRecordService!.Add(timeRecord);

            if (!CancelIntegration)
            {
                RegisterPontoIntegration(dateTime, pontoConfig);
            }

            Console.WriteLine($"{TypeRecord} Registered");
            RunInfoCommand();
        }

        private void RegisterPontoIntegration(DateTime dateTime, PontoConfiguration pontoConfig) {
            var dateTimePlus10Min = dateTime.AddSeconds(60 * 10);

            var options = new PontoIntegrationOptions(
                pontoConfig.UserName,
                pontoConfig.Password,
                pontoConfig.ExpectedProfileName,
                dateTime,
                TypeRecord.ToString()
            );

            var interation = new PontoIntegration(options);

            var todayInitial = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
            var todayEnd = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
            var line = DbContext!.TimeRecords.Where(t => t.Time >= todayInitial && t.Time <= todayEnd && t.Type == TypeRecord).Count() - 1;

            interation.Login();
            interation.LogWork(
            line,
                dateTime.ToString("HH:mm"),
                dateTimePlus10Min.ToString("HH:mm"));
        }

        private void RunInfoCommand()
        {
            new InfoCommand().Run(Host, DbContext!);
        }
    }
}
