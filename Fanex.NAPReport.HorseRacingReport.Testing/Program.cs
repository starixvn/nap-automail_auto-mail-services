using System;
using System.IO;
using Fanex.Data.Configuration;
using Fanex.NAPReport.HorseRacingReport.Business;
using Fanex.NAPReport.HorseRacingReport.Business.Criteria;
using Fanex.Service.Core.Extensions;
using RazorEngine;
using Fanex.NAPReport.HorseRacingReport.Business.Service;

namespace Fanex.NAPReport.HorseRacingReport.Testing
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            Console.WriteLine("Starting ProcessHorseRacingService at: " + DateTime.Now.ToString());

            IHorseRacingService service = new HorseRacingService();
            TimeRangeCriteria timeRangeCriteria = AssignRangeDate(service);

            service.GetData(timeRangeCriteria);
            string path = service.ReportZipFolderPath;

            var root = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = root + AppSettings.EmailTemplate;
            string template = File.ReadAllText(filePath);
            var emailContent = Razor.Parse(template, timeRangeCriteria);
            string emailSubject = string.Format(
                AppSettings.MailSubject,
                timeRangeCriteria.FromDate.ToShortFormatedDateWithSlash(),
                timeRangeCriteria.ToDate.ToShortFormatedDateWithSlash());
            string emailTitle = AppSettings.MailTitle;

            Console.WriteLine("Done ProcessHorseRacingService at: " + DateTime.Now.ToString());
            Console.ReadLine();
        }

        private static TimeRangeCriteria AssignRangeDate(IHorseRacingService service)
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