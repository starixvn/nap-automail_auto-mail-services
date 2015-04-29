// -----------------------------------------------------------------------
// <copyright file="AutomailService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service
{
    using System;
    using System.IO;
    using System.ServiceProcess;
    using Fanex.Data.Configuration;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.NAPReport.HorseRacingReport.Business;

    /// <summary>
    /// Auto mail service.
    /// </summary>
    public partial class AutoMailService : ServiceBase
    {
        /// <summary>
        /// The timer.
        /// </summary>

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