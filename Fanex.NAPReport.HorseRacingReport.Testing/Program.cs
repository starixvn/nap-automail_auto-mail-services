using System;
using System.IO;
using Fanex.Data.Configuration;
using Fanex.NAPReport.HorseRacingReport.Business;
using Fanex.NAPReport.HorseRacingReport.Business.Constants;
using Fanex.NAPReport.HorseRacingReport.Business.Criteria;
using Fanex.Service.Core.Extensions;
using RazorEngine;


namespace Fanex.NAPReport.HorseRacingReport.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            Console.WriteLine("Starting ProcessHorseRacingService at: " + DateTime.Now.ToString());

            ProcessHorseRacingService service = new ProcessHorseRacingService();
            TimeRangeCriteria timeRangeCriteria = AssignRangeDate(service);

            service.ProcessHorseRacingReportService(timeRangeCriteria);
            string path = service.ReportZipFolderPath;

            var root = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = root + EmailTemplateConstants.RacingReportEmailTemplate;
            string template = File.ReadAllText(filePath);
            var emailContent = Razor.Parse(template, timeRangeCriteria);
            string emailSubject = string.Format(
                AppSettings.Subject,
                timeRangeCriteria.FromDate.ToShortFormatedDateWithSlash(),
                timeRangeCriteria.ToDate.ToShortFormatedDateWithSlash());
            string emailTitle = AppSettings.Title;

            Console.WriteLine("Done ProcessHorseRacingService at: " + DateTime.Now.ToString());
            Console.ReadLine();
        }

        private static TimeRangeCriteria AssignRangeDate(ProcessHorseRacingService service)
        {
            TimeRangeCriteria timeRangeCriteria = new TimeRangeCriteria();

            bool isRunningDay = IsRunningDay();

            if (isRunningDay)
            {
                timeRangeCriteria = service.GetTimeRangeCriteria();
            }
            else
            {
                timeRangeCriteria.FromDate = DateTime.Today.AddDays(-4);
                timeRangeCriteria.ToDate = DateTime.Today.AddDays(-1);
            }

            return timeRangeCriteria;
        }

        private static bool IsRunningDay()
        {
            DateTime currentTime = DateTime.Now;
            return currentTime.DayOfWeek == DayOfWeek.Thursday || currentTime.DayOfWeek == DayOfWeek.Monday;
        }
    }
}
