using System;
using System.IO;
using Fanex.Data.Configuration;
using Fanex.NAPReport.SMALoss.Business.Process;

namespace Fanex.NAPReport.SMALoss.Testing
{
    internal class Program
    {
        private const string TemplateUrl = @"/Templates/SMALossTemplate.cshtml";

        private static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            var lossProcess = new SMALossProcess();
            lossProcess.NotifyEmail(TemplateUrl);
        }
    }
}