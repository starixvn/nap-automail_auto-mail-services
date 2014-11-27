// -----------------------------------------------------------------------
// <copyright file="ReportingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Franklin.Nguyen" date="2014.04.25">Created</change>
// </history>
namespace Fanex.ReportingService.Business
{
    using System;
    using System.IO;
    using System.ServiceProcess;
    using System.Timers;
    using Fanex.Data.Configuration;
    using Fanex.ReportingService.Business.Common;
    using Fanex.ReportingService.Business.Enums;
    using Fanex.ReportingService.Business.Service;
    using Timer = System.Timers.Timer;

    /// <summary>
    /// Reporting service.
    /// </summary>
    public partial class ReportingService : ServiceBase
    {
        /// <summary>
        /// The timer.
        /// </summary>
        private static Timer timer;

        private static ReportingServiceQueue serviceQueue;

        public ReportingService()
        {
            InitializeComponent(AppSettings.ServiceName);
        }

        /// <summary>
        /// Execute when service start.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            timer = new Timer();
            timer.Interval = AppSettings.Interval;
            timer.Elapsed += new ElapsedEventHandler(OnInterval);
            timer.AutoReset = true;
            timer.Enabled = true;

            AppSettings.OnProcessing = false;
            AppSettings.ForceRunningOnProcessing = false;

            serviceQueue = new ReportingServiceQueue();
        }

        /// <summary>
        /// Execute when service stop.
        /// </summary>
        protected override void OnStop()
        {
            DbSettingProviderManager.Current.Start();
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            timer.Stop();
            timer = null;
            serviceQueue = null;
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
            TimeSpan executeTime = new TimeSpan(AppSettings.RunAtHours, AppSettings.RunAtMinutes, 0);
            var currentTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
            int compareResult = TimeSpan.Compare(currentTime, executeTime);
            if (compareResult >= 0)
            {
                ProcessBusiness();
            }

            try
            {
                if (!AppSettings.ForceRunningOnProcessing && AppSettings.ForceRunning)
                {
                    AppSettings.ForceRunningOnProcessing = true;
                    DbSettingProviderManager.Current.Restart();
                    serviceQueue.ProcessBusiness(TaskType.Urgently);
                }
            }
            catch (Exception ex)
            {
                AppSettings.OnProcessing = false;
                LogExtensions.ExceptionHandler(ex);
            }
        }

        /// <summary>
        /// Called when [running].
        /// </summary>
        private static void ProcessBusiness()
        {
            try
            {
                if (!serviceQueue.IsRunning())
                {
                    if (!serviceQueue.HasTaskAlreadyRunToday())
                    {
                        DbSettingProviderManager.Current.Restart();
                        serviceQueue.ProcessBusiness(TaskType.Daily);
                    }
                }
            }
            catch (Exception ex)
            {
                AppSettings.OnProcessing = false;
                LogExtensions.ExceptionHandler(ex);
            }
        }
    }
}