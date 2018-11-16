using System;
using System.IO;
using Fanex.Data.Configuration;
using Fanex.NAPReport.WinLossByProducts.Business;
using Fanex.NAPReport.WinLossByProducts.Business.Services;
using Fanex.Service.Core.Configuration;
using Newtonsoft.Json;

namespace Fanex.ProductsWinLoss.Executing
{
    public static class Program
    {
        public static void Main()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\WinLossByProducts\Settings\OverallWinLoss.Settings.xml");
            configurationParser.Parse<AppSettings>();

            var service = new MailingService();
            var jobMessage = service.Execute();

            var json = JsonConvert.SerializeObject(jobMessage, Formatting.Indented);            
            Console.Write(json);
            Environment.Exit(0);
        }
    }
}