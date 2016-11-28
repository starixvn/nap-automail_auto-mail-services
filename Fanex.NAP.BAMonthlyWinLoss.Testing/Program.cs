namespace Fanex.NAPReport.BAMonthlyWinLoss.Testing
{
    using System;
    using System.IO;
    using Fanex.Data.Configuration;
    using Fanex.NAPReport.BAMonthlyWinLoss.Business.Service;

    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            Console.WriteLine("Running BetAggregatorService");

            IMailingService service = new MailingService();
            var mail = service.SendMailToVIPGroup();

            Console.WriteLine("Done at " + DateTime.Now.ToString());
        }
    }
}