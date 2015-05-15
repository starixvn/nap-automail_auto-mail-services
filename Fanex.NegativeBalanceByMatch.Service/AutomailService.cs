using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using Fanex.Data.Configuration;
using Fanex.NegativeBalanceByMatch.Business;
using Fanex.NegativeBalanceByMatch.Business.Business;
using Fanex.Service.Core.Extensions;

namespace Fanex.NegativeBalanceByMatch.Service
{
    internal partial class AutomailService : ServiceBase
    {
        /// <summary>
        /// The timer.
        /// </summary>
        private static Timer timer;

        private static NegativeBalanceByMatchService service;

        public AutomailService()
        {
            InitializeComponent(AppSettings.ServiceName);
        }

        /// <summary>
        /// Implement when service start.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            DbSettingProviderManager.Current.Start();
            timer = new Timer();
            timer.Interval = AppSettings.Interval;
            timer.Elapsed += new ElapsedEventHandler(OnInterval);
            timer.AutoReset = true;
            timer.Enabled = true;

            AppSettings.OnProcessing = false;
            AppSettings.ForceRunningOnProcessing = false;
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            timer.Stop();
            timer = null;
            service = null;
            AppSettings.OnProcessing = false;
            GC.Collect();
        }

        /// <summary>
        /// Called when [interval].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        private static void OnInterval(object source, ElapsedEventArgs e)
        {
            try
            {
                if (!AppSettings.OnProcessing)
                {
                    AppSettings.OnProcessing = true;
                    service = new NegativeBalanceByMatchService();
                    service.ProcessBusiness();
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