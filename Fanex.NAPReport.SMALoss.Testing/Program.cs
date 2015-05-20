using System;
using System.IO;
using Fanex.Data.Configuration;
using Fanex.NAPReport.AutoMailReport.Service;
using Fanex.NAPReport.SMALoss.Business.Business;

namespace Fanex.NAPReport.SMALoss.Testing
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //System.ServiceProcess.ServiceBase[] ServicesToRun;
            //ServicesToRun = new System.ServiceProcess.ServiceBase[] { new AutoMailService() };
            //System.ServiceProcess.ServiceBase.Run(ServicesToRun);

            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();

            var lossProcess = new SMALossProcess();
            lossProcess.NotifyEmail();
        }
    }
}