namespace Fanex.NAPReport.WinLossByProducts.Testing
{
    using System;
    using Fanex.Data.Configuration;
    using Fanex.NAPReport.WinLossByProducts.Business.Services;
    using Fanex.Service.Core.MailService;

    public class Program
    {
        public static void Main(string[] args)
        {            
            DbSettingProviderManager.Current.Start();         
            Console.WriteLine("Running Win Loss By Products Report");

            IMailingServiceBase service = new MailingService();
            var mail = service.SendMailToVIPGroup();                       
            
            Console.WriteLine("Done at " + DateTime.Now.ToString());            
        }
    }
}