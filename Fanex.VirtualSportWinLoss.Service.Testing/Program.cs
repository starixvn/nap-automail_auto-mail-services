namespace Fanex.NAPReport.TurnoverWatcher.Service.Testing
{
    using System;
    using System.IO;
    using Fanex.Data.Configuration;
    using Fanex.NAPReport.VirtualSportWinLoss.Business;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Implement;
    using Fanex.Service.Core.Configuration;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Running VirtualSportWinLoss");            
            DbSettingProviderManager.Current.Start();

            MailingService service = new MailingService();
            var mail = service.SendMailToVIPGroup();

            Console.WriteLine("Done at " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:s tt"));            
        }
    }
}