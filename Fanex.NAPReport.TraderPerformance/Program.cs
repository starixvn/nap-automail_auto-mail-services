using Fanex.Data.Configuration;
using Fanex.NAPReport.TraderPerformance.Business;
using Fanex.NAPReport.TraderPerformance.Business.Services;
using Fanex.Service.Core.Configuration;
using System;
using System.IO;

namespace Fanex.NAPReport.TraderPerformance
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                DbSettingProviderManager.Current.Start();

                string root = AppDomain.CurrentDomain.BaseDirectory;
                ConfigurationParser configurationParser = new ConfigurationParser(root + @"SiteConfig\Settings\Settings.xml");
                configurationParser.Parse<AppSettings>();

                Console.WriteLine("Start running trader performance report!");

                var service = new TraderPerformanceService();
                var exportService = new ExportService();

                var fromDate = AppSettings.FromDate;
                var toDate = AppSettings.ToDate;

                if (AppSettings.IsTest)
                {
                    fromDate = toDate = DateTime.Now.AddDays(-13);
                }

                for (var reportDate = fromDate.Date; reportDate.Date <= toDate.Date; reportDate = reportDate.AddDays(1))
                {
                    var mainReport = service.GetReport(reportDate);
                    var detailReport = service.GetDetailReport(reportDate);
                    exportService.Export(mainReport, detailReport, reportDate);
                }

                Console.WriteLine("Done at " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);                
            }

            Console.ReadLine();
        }
    }
}
