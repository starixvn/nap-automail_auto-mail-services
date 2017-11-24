using System;
using System.IO;
using Fanex.Data.Configuration;
using Fanex.MailDelivery.Core.Models;
using Fanex.NAPReport.SMALoss.Business;
using Fanex.NAPReport.SMALoss.Business.Services;
using Fanex.Service.Core.Configuration;
using Newtonsoft.Json;

namespace Fanex.SMAMaxLoss.Executing
{
    public static class Program
    {
        public static void Main()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\SMALoss\Settings\SMALoss.Settings.xml");
            configurationParser.Parse<AppSettings>();

            var service = new MailingService();
            var mail = service.SendMailToVIPGroup();
            if (mail.JobStatus == JobStatus.Success)
            {
                service.SendMailToNormalGroup();
            }

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