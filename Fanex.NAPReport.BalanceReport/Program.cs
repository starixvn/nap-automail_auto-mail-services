namespace Fanex.NAPReport.BalanceMonthlyReport.Business
{
    using System;
    using System.IO;
    using Fanex.Data.Configuration;
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Service.Mailing;

    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                DbSettingProviderManager.Current.Start();

                IMailingService mailingService = new MailingService();
                var mail = mailingService.SendMailCreditTermToVIPGroup();
                mail = mailingService.SendMailSuperBalanceToVIPGroup();
                mail = mailingService.SendMailDirectBalanceToVIPGroup();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
