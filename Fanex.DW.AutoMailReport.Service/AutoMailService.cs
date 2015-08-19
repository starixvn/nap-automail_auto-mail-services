namespace Fanex.DW.AutoMailReport.Service
{
    using System;
    using System.IO;
    using System.ServiceProcess;
    using Fanex.Data.Configuration;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.Service.Core.Extensions;

    public partial class AutoMailService : ServiceBase
    {
        public AutoMailService()
        {
            InitializeComponent(AppSettings.ServiceName);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

                DbSettingProviderManager.Current.Start();

                MailClientHandler.Start();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        protected override void OnStop()
        {
            MailClientHandler.Stop();
            GC.Collect();
        }
    }
}