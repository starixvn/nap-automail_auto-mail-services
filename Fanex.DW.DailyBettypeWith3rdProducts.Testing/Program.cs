using System;
using System.IO;
using Fanex.Data.Configuration;
using Fanex.DW.DailyBettypeWith3rdProducts.Business.Service;
using Fanex.DW.DailyBettypeWith3rdProducts.Business.Resource;
using Fanex.MailDelivery.Client.Models;
using System.Collections.Generic;

namespace Fanex.DW.DailyBettypeWith3rdProducts.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            Console.WriteLine("Running DailyBettypeWith3rdProductsService");

            DailyBettypeWith3rdProductsService service = new DailyBettypeWith3rdProductsService();
            service.GenerateReport(false);
            Mail mail = new Mail()
            {
                Subject = string.Format(Label.DailyBettype3rdProductMailSubject, DateTime.Now.ToString("yyyy-MM-dd")),
                Title = string.Format(Label.DailyBettype3rdProductMailSubject, DateTime.Now.ToString("yyyy-MM-dd")),
                Body = Label.DailyBettype3rdProductMailBody
            };

            mail.Attachments = new List<MailAttachment>();
            mail.Attachments.Add(new MailAttachment(@service.GetFileName, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));
            service.GenerateReport(true);
            mail.Attachments.Add(new MailAttachment(@service.GetFileName, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));


            Console.WriteLine("Done at " + DateTime.Now.ToString());
            Console.ReadLine();
        }
    }
}
