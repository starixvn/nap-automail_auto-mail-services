using Fanex.BalanceMonthlyReport.Business.Service;
using Fanex.Data.Configuration;
using Fanex.MailDelivery.Core.Models;
using Fanex.NAPReport.BalanceMonthlyReport.Business;
using Fanex.NAPReport.BalanceMonthlyReport.Business.Service.Mailing;
using Fanex.Service.Core.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Fanex.BalanceMonthlyReportCreditTerm.Executing
{
    public static class Program
    {
        public static void Main()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\BalanceMonthlyReport\Settings\BalanceMonthlyReport.Settings.xml");
            configurationParser.Parse<AppSettings>();

            var service = new CreditTermNotificationImp();
            var jobResult = service.Execute();

            var json = JsonConvert.SerializeObject(jobResult);
            Console.Write(json);
            Environment.Exit(0);
        }
    }
}