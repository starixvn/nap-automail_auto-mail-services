using System;
using System.Collections.Generic;
using System.IO;
using Fanex.Data.Configuration;
using Fanex.DW.DailyBettypeWith3rdProducts.Business.Resource;
using Fanex.DW.DailyBettypeWith3rdProducts.Business.Service;
using Fanex.MailDelivery.Client.Models;
using Fanex.DW.DailyBettypeWith3rdProducts.Business.Jobs;
using Fanex.DW.DailyBettypeWith3rdProducts.Business.Model;

namespace Fanex.DW.DailyBettypeWith3rdProducts.Testing
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            Console.WriteLine("Running DailyBettypeWith3rdProductsService");

            IDailyBettypeWith3rdProductJob job = new DailyBettypeWith3rdProductJob();
            job.ProcessExportDailyBettypeWith3rdProducts(false);
            Console.WriteLine("Done at " + DateTime.Now.ToString());
            Console.ReadLine();
        }
    }
}