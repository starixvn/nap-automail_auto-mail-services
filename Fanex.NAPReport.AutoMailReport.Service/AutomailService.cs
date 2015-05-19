// -----------------------------------------------------------------------
// <copyright file="AutomailService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service
{
    using System;
    using System.IO;
    using System.Timers;
    using Fanex.Data.Configuration;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.NAPReport.HorseRacingReport.Business;
    using Fanex.NAPReport.SMALoss.Business.Business;
    using Fanex.Service.Core.Extensions;

    /// <summary>
    /// Auto mail service.
    /// </summary>
    public partial class AutoMailService : System.ServiceProcess.ServiceBase
    {
        private const string TemplateUrl = @"/SiteConfig/SMALoss/SMALossTemplate.cshtml";
        private TimeSpan _startTime;
        private Timer _timer = null;
        private ISMALossProcess _smaLossProcess = null;

        public AutoMailService()
        {
            InitializeComponent(AppSettings.ServiceName);
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DbSettingProviderManager.Current.Start();
            MailClientHandler.Start();
            this._smaLossProcess = new SMALossProcess();
        }

        protected override void OnStart(string[] args)
        {
            ResetTimer();
        }

        protected override void OnStop()
        {
            MailClientHandler.Stop();
            this._timer.Stop();
            this._timer = null;
            this._smaLossProcess = null;
            AppSettings.OnProcessing = false;
            GC.Collect();
        }

        /// <summary>
        /// Resets the timer.
        /// </summary>
        private void ResetTimer()
        {
            this._timer = new Timer();
            this._timer.Interval = AppSettings.Interval;
            this._timer.Elapsed += new ElapsedEventHandler(this.CallService);
            this._timer.AutoReset = true;
            this._timer.Enabled = true;
            AppSettings.OnProcessing = false;
        }

        /// <summary>
        /// Calls the service.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        private void CallService(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!AppSettings.OnProcessing)
                {
                    AppSettings.OnProcessing = true;

                    if (this._smaLossProcess != null)
                    {
                        this._smaLossProcess.NotifyEmail(TemplateUrl);
                    }

                    AppSettings.OnProcessing = false;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                AppSettings.OnProcessing = false;
                throw;
            }
        }
    }
}