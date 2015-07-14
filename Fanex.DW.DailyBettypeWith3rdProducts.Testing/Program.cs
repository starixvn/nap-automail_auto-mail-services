using System;
using System.IO;
using Fanex.Data.Configuration;
using Fanex.DW.DailyBettypeWith3rdProducts.Business.Jobs;
using Fanex.MailDelivery.Client.Handlers;

namespace Fanex.DW.DailyBettypeWith3rdProducts.Testing
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();
            MailClientHandler.Start();

            Console.WriteLine("Running DailyBettypeWith3rdProductsService");

            var job = new DailyBettypeWith3rdProductJob();

            string subject = "Bettype With 3rd Products";
            job.ExportBettypeWith3rdProducts(true, subject);

            MailClientHandler.Stop();

            Console.WriteLine("Done at " + DateTime.Now.ToString());
            Console.ReadLine();
        }
    }
}