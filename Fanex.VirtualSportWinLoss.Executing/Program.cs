using System;
using System.IO;
using Fanex.Data.Configuration;
using Fanex.MailDelivery.Core.Models;
using Fanex.NAPReport.VirtualSportWinLoss.Business;
using Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Implement;
using Fanex.Service.Core.Configuration;
using Newtonsoft.Json;

namespace Fanex.VirtualSportWinLoss.Executing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\VirtualSportWinLoss\Settings\VirtualSportWinLoss.Settings.xml");
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