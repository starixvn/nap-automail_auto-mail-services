using System.ServiceProcess;
using System.Timers;
using System.IO;
using System;
using Fanex.Data.Configuration;
using Fanex.MailDelivery.Client.Handlers;
using Fanex.NegativeBalanceByMatch.Business;
using Fanex.Service.Core.Extensions;
using Fanex.NegativeBalanceByMatch.Business.Business;

namespace Fanex.NegativeBalanceByMatch.Service
{
    internal partial class AutoMailService : ServiceBase
    {
        public AutoMailService()
        {
            InitializeComponent(AppSettings.ServiceName);
        }

        protected override void OnStart(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            DbSettingProviderManager.Current.Start();
            
            MailClientHandler.Start();
        }

        protected override void OnStop()
        {
            MailClientHandler.Stop();
            GC.Collect();
        }
    }
}