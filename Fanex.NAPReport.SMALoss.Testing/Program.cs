using System;
using System.IO;
using Fanex.Data.Configuration;
using Fanex.MailDelivery.Client.Handlers;
using Fanex.NAPReport.SMALoss.Business.Services;
using Fanex.NAPReport.SMALoss.Business;

namespace Fanex.NAPReport.SMALoss.Testing
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();
            MailClientHandler.Start();

            var lossProcess = new MailingService();
            var mail = lossProcess.SendMailToVIPGroup();

            mail.Mail.MailingListId = AppSettings.FunctionId;
            MailClientHandler.SendMail(mail.Mail);
            MailClientHandler.Stop();
        }
    }
}