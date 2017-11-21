using System;
using System.IO;
using Fanex.Data.Configuration;
using Fanex.MailDelivery.Core.Models;
using Fanex.NAPReport.ReportingServices.Business;
using Fanex.NAPReport.ReportingServices.Business.Service;
using Fanex.Service.Core.Configuration;
using Newtonsoft.Json;

namespace Fanex.ReportingServices.Executing
{
    public static class Program
    {
        public static void Main()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\ReportingServices\Settings\ReportingService.Settings.xml");
            configurationParser.Parse<AppSettings>();

            var service = new MailingService();
            var mail = service.SendMailToVIPGroup();
            service.SendMailToNormalGroup();
            JobMessage jobResult = new JobMessage
            {
                Mail = mail.Mail,
                JobStatus = mail.JobStatus
            };

            var json = JsonConvert.SerializeObject(jobResult);
            Console.Write(json);
            Environment.Exit(0);
        }
    }
}