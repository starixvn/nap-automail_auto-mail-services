namespace Fanex.NAPReport.HorseRacingReport.Testing
{
    using System;
    using System.IO;
    using Fanex.Data.Configuration;
    using Fanex.NAPReport.HorseRacingReport.Business.Service;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            Console.WriteLine("Starting ProcessHorseRacingService at: " + DateTime.Now.ToString());

            IMailingService service = new MailingService();
            var mail = service.SendMailToVIPGroup();

            Console.WriteLine("Done ProcessHorseRacingService at: " + DateTime.Now.ToString());
            Console.ReadLine();
        }
    }
}