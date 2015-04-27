using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using Fanex.OandaExChangeRate.Business;
using Fanex.OandaExChangeRate.Business.Common;
using Fanex.Data.Configuration;
using Fanex.OandaExChangeRate.Business.DataAccess;

namespace Fanex.OandaExChangeRate.Service
{
    public partial class AutoMailService : ServiceBase
    {
        /// <summary>
        /// The timer.
        /// </summary>
        private static Timer timer;

        private static ProcessExChangeRate service;

        public AutoMailService()
        {            
            InitializeComponent("");
        }

        /// <summary>
        /// On start service.
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
            timer.Stop();
            timer = null;
            service = null;

            AppSettings.OnProcessing = false;
            AppSettings.ForceRunningOnProcessing = false;

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
                service = new ProcessExChangeRate();
                DateTime executeDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, AppSettings.RunAtHours, AppSettings.RunAtMinutes, 0);
                DateTime currentDate = DateTime.Now;
                //     A signed number indicating the relative values of t1 and t2.Value Type Condition
                //     Less than zero t1 is earlier than t2. Zero t1 is the same as t2. Greater
                //     than zero t1 is later than t2.
                int compareResult = DateTime.Compare(currentDate, executeDate);

                if (compareResult < 0)
                {
                    SQLiteDataAccess _log = new SQLiteDataAccess();
                    compareResult = _log.GetTodayLogId();
                }

                if (compareResult >= 0)
                {
                    if (!AppSettings.OnProcessing)
                    {
                        AppSettings.OnProcessing = true;
                        service.OnRunningDaily();
                        AppSettings.OnProcessing = false;
                    }
                }

                if (!AppSettings.ForceRunningOnProcessing && AppSettings.ForceRunning)
                {
                    AppSettings.ForceRunningOnProcessing = true;
                    AppSettings.OnProcessing = true;
                    service.OnRunningUrgently();
                    AppSettings.OnProcessing = false;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                AppSettings.OnProcessing = false;
            }
            finally
            {
                service = null;
                GC.Collect();                
            }
        }
    }
}

