using System;
using System.IO;
using Fanex.CreditMovement.Business;
using Fanex.CreditMovement.Business.Service;
using Fanex.Data.Configuration;
using Fanex.MailDelivery.Core.Models;
using Fanex.Service.Core.Configuration;
using Newtonsoft.Json;

namespace Fanex.CreditMovement.Executing
{
    public static class Program
    {
        public static void Main()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\CreditMovement\Settings\CreditMovement.Settings.xml");
            configurationParser.Parse<AppSettings>();

            var service = new MailingService();
            var mail = service.SendMailToVIPGroup();
            service.SendMailToNormalGroup();
            JobMessage jobResult = new JobMessage
            {
                Mail = mail.Mail,
                JobStatus = mail.JobStatus
            };

            var json = JsonConvert.SerializeObject(jobResult, Formatting.Indented);

            Console.Write(json);
            Environment.Exit(0);
        }
    }
}