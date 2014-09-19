using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using Fanex.Data.Configuration;
using Fanex.Template.Business.Service;
using Fanex.Template.Business.Business;
using Fanex.Template.Business.Common;

namespace Fanex.Template.Service
{
    public partial class AutoMailService : ServiceBase
    {
        /// <summary>
        /// The timer.
        /// </summary>
        private static Timer timer;

        private static ITemplateService service;

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

            service = new TemplateService();
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
