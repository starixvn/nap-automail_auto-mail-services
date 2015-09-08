namespace Fanex.DW.DailyBettypeWith3rdProducts.Testing
{
    using System;
    using System.IO;
    using Fanex.Data.Configuration;
    using Fanex.DW.DailyBettypeWith3rdProducts.Business;
    using Fanex.DW.DailyBettypeWith3rdProducts.Business.Service;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.Service.Core.Configuration;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();
            MailClientHandler.Start();

            ConfigurationParser configurationParser = new ConfigurationParser(@"SiteConfig\BettypeWith3rdProducts\Settings\Bettype_With_3rd.Settings.xml");
            configurationParser.Parse<AppSettings>();

            Console.WriteLine("Running DailyBettypeWith3rdProductsService");

            var job = new MailingService();

            string subject = "Bettype With 3rd Products";
            job.ExportBettypeWith3rdProducts(true, subject);

            MailClientHandler.Stop();

            Console.WriteLine("Done at " + DateTime.Now.ToString());
            Console.ReadLine();
        }
    }
}