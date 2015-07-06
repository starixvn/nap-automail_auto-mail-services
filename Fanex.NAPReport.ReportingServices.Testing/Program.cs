namespace Fanex.NAPReport.ReportingServices.Testing
{
    using System;
    using System.IO;
    using Fanex.Data.Configuration;
    using Fanex.NAPReport.ReportingServices.Business.Service;

    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();
            Console.WriteLine("Start running reporting services!");

            var serviceProcess = new ServiceProcess();
            var combanyBalance = new CompanyBalanceProcessData(serviceProcess);
            var companyStatement = new CompanyStatementProcessData(serviceProcess);
            var dailyTurnover = new DailyTurnoverProcessData(serviceProcess);
            var superYesterdayBalance = new SuperYesterdayBalanceProcessData(serviceProcess);
            var matchWinLoss2a = new MatchWinLoss2aProcessData(serviceProcess);

            var currentDate = new DateTime(2015, 6, 18); //// DateTime.Now;
            serviceProcess.RunService(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0, 0, 0));
            Console.WriteLine("Done at " + DateTime.Now.ToString());
            Console.ReadLine();
        }
    }
}