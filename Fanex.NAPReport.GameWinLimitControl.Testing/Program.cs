namespace Fanex.NAPReport.GameWinLimitControl.Testing
{
    using Fanex.Data.Configuration;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.NAPReport.GameWinLimitControl.Business.ServiceFactory;

    public class Program
    {
        private static void Main(string[] args)
        {
            DbSettingProviderManager.Current.Start();
            MailClientHandler.Start();

            var mail = new MailingServiceFactory();
            var f = mail.NotifyMailVipGroup();          

            MailClientHandler.Stop();
        }
    }
}
