using System;
using System.IO;
using Fanex.Data.Configuration;
using Fanex.NAPReport.HorseRacingReport.Business;
using Fanex.NAPReport.HorseRacingReport.Business.Criteria;
using Fanex.NAPReport.HorseRacingReport.Business.Service;
using Fanex.Service.Core.Constants;
using Fanex.Service.Core.Extensions;
using Fanex.Service.Core.Model;
using RazorEngine;

namespace Fanex.NAPReport.HorseRacingReport.Testing
{
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