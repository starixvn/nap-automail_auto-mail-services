namespace Fanex.NAPReport.ReportingServices.Testing
{
    using Fanex.Data.Configuration;
    using Fanex.NAPReport.ReportingServices.Business;
    using Fanex.NAPReport.ReportingServices.Business.Service;
    using Fanex.Service.Core.Configuration;
    using System;
    using System.IO;

    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            string root = AppDomain.CurrentDomain.BaseDirectory;
            ConfigurationParser configurationParser = new ConfigurationParser(root + @"SiteConfig\ReportingServices\Settings\ReportingService.Settings.xml");
            configurationParser.Parse<AppSettings>();

            Console.WriteLine("Start running reporting services!");

            var serviceProcess = new ServiceProcess();
            var combanyBalance = new CompanyBalanceProcessData(serviceProcess, false);
            var combanyYesterdayBalance = new CompanyBalanceProcessData(serviceProcess, true);
            var companyStatement = new CompanyStatementProcessData(serviceProcess);
            var dailyTurnover = new DailyTurnoverProcessData(serviceProcess);
            var superYesterdayBalance = new SuperYesterdayBalanceProcessData(serviceProcess);

            var currentDate = DateTime.Today.AddDays(-1);
            if (AppSettings.IsTest)
            {
                currentDate = AppSettings.WinLossDate;
            }

            serviceProcess.RunService(currentDate);
            Console.WriteLine("Done at " + DateTime.Now.ToString());
        }
    }
}