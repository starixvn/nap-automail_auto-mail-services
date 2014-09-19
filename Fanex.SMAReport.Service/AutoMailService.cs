using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using Fanex.SMAReport.Business;
using Fanex.SMAReport.Business.Service;
using System.IO;
using Fanex.Data.Configuration;
using Fanex.SMAReport.Business.Common;

namespace Fanex.SMAReport.Service 
{
    public partial class AutoMailService : ServiceBase
    {
        /// <summary>
        /// The timer.
        /// </summary>
        private static Timer timer;

        private static ISMAReportService service;

        public AutoMailService()
        {
            InitializeComponent(AppSettings.ServiceName);
        }

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

            service = new SMAReportService();
        }

        protected override void OnStop()
        {
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
                    service.ProcessResetWinning();
                    AppSettings.OnProcessing = false;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                AppSettings.OnProcessing = false;
            }
        }
    }
}
