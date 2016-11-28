namespace Fanex.NAPReport.WinLossByProducts.Testing
{
    using System;
    using Fanex.Data.Configuration;
    using Fanex.NAPReport.WinLossByProducts.Business;
    using Fanex.NAPReport.WinLossByProducts.Business.Services;
    using Fanex.Service.Core.Configuration;
    using MailDelivery.Client;

    public class Program
    {
        public static void Main(string[] args)
        {
            DbSettingProviderManager.Current.Start();
            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\WinLossByProducts\Settings\OverallWinLoss.Settings.xml");
            configurationParser.Parse<AppSettings>();

            Console.WriteLine("Running Win Loss By Products Report");
            MailClientHandler.Start();

            MailingService service = new MailingService();
            var mail = service.SendMailToVIPGroup();

            MailClientHandler.Stop();
            Console.WriteLine("Done at " + DateTime.Now.ToString());
        }
    }
}