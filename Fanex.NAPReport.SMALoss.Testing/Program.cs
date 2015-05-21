using System;
using System.IO;
using Fanex.Data.Configuration;
using Fanex.MailDelivery.Client.Handlers;
using Fanex.NAPReport.AutoMailReport.Service;
using Fanex.NAPReport.SMALoss.Business.Business;

namespace Fanex.NAPReport.SMALoss.Testing
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();
            MailClientHandler.Start();

            var lossProcess = new SMALossProcess();
            lossProcess.NotifyEmail();
            MailClientHandler.Stop();
        }
    }
}