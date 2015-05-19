namespace Fanex.HorseTransaction3rdServices
{
    using System;
    using System.IO;
    using System.ServiceProcess;
    using Fanex.Data.Configuration;
    using Fanex.MailDelivery.Client.Handlers;

    public partial class AutoMailService : ServiceBase
    {
        public AutoMailService()
        {
            InitializeComponent();
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
