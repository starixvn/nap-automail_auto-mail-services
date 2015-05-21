namespace Fanex.NAP.BAMonthlyWinLoss.Testing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.Data.Configuration;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAP.BAMonthlyWinLoss.Business;
    using Fanex.NAP.BAMonthlyWinLoss.Business.Resources;
    using Fanex.NAP.BAMonthlyWinLoss.Business.Service;

    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            Console.WriteLine("Running BetAggregatorService");

            var service = new BAReportService();
            var isTesting = AppSettings.IsTesting;
            var testedDate = AppSettings.TestedDate;
            var winLossDate = isTesting ? testedDate : DateTime.Now.AddDays(-1);

            service.GenerateReport(winLossDate);

            string filePath = AppDomain.CurrentDomain.BaseDirectory + AppSettings.TemplateDirectory;
            string template = File.ReadAllText(filePath);
            var emailContent = string.Format(template, winLossDate);

            var mail = new Mail()
            {
                Subject = string.Format(Label.Subject, winLossDate.ToShortDateString()),
                Title = Label.Title,
                Body = emailContent
            };
            mail.Attachments = new List<MailAttachment>();
            mail.Attachments.Add(new MailAttachment(@service.DirectoryOfFileName, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));


            Console.WriteLine("Report's generated at " + service.DirectoryOfFileName);

            Console.WriteLine("Done at " + DateTime.Now.ToString());
        }
    }
}
