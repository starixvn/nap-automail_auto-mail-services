using System;
using System.IO;
using Fanex.Data.Configuration;
using Fanex.MailDelivery.Core.Models;
using Fanex.NAPReport.BalanceMonthlyReport.Business;
using Fanex.NAPReport.BalanceMonthlyReport.Business.Service.Mailing;
using Fanex.Service.Core.Configuration;
using Newtonsoft.Json;

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

            var service = new MailingService();
            var mail = service.SendMailDirectBalanceToVIPGroup();
            service.SendMailToDirectBalanceNormalGroup();
            JobMessage jobResult = new JobMessage
            {
                Mail = mail,
                JobStatus = JobStatus.Success
            };

            var json = JsonConvert.SerializeObject(jobResult);
            Console.Write(json);
            Environment.Exit(0);
        }
    }
}