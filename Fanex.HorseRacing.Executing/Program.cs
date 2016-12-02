using System;
using System.IO;
using Fanex.Data.Configuration;
using Fanex.MailDelivery.Core.Models;
using Fanex.NAPReport.HorseRacingReport.Business;
using Fanex.NAPReport.HorseRacingReport.Business.Service;
using Fanex.Service.Core.Configuration;
using Newtonsoft.Json;

namespace Fanex.HorseRacing.Executing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\HorseRacingReport\Settings\HorseRacingReport.Settings.xml");
            configurationParser.Parse<AppSettings>();

            var service = new MailingService();
            var mail = service.SendMailToVIPGroup();
            service.SendMailToNormalGroup();
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