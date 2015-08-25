namespace Fanex.NAPReport.BalanceMonthlyReport.Business
{
    using System;
    using Fanex.Data.Configuration;
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Service.Balance;
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Service.Mailing;

    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
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
