namespace Fanex.NAPReport.TurnoverWatcher.Service.Testing
{
    using System;
    using System.IO;
    using Fanex.Data.Configuration;
    using Fanex.NAPReport.TurnoverWatcher.Business.Service;
    using MailDelivery.Client;

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Running TurnoverWatcher");

                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                DbSettingProviderManager.Current.Start();
                MailClientHandler.Start();

                var service = new MailingService();
                service.SendMailToVIPGroup();

                MailClientHandler.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
    }
}