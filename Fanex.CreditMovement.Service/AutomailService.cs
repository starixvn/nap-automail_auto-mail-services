// -----------------------------------------------------------------------
// <copyright file="AutomailService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CreditMovement.Service
{
    using System;
    using System.IO;
    using System.ServiceProcess;
    using System.Timers;
    using Fanex.CreditMovement.Business;
    using Fanex.CreditMovement.Business.Constants;
    using Fanex.CreditMovement.Business.Criteria;
    using Fanex.CreditMovement.Business.Model;
    using Fanex.CreditMovement.Business.Service;
    using Fanex.Data.Configuration;
    using Fanex.Service.Core.Extensions;

    /// <summary>
    /// Auto email service.
    /// </summary>
    public partial class AutomailService : ServiceBase
    {
        /// <summary>
        /// The timer.
        /// </summary>
        private static Timer timer;

        private static ICreditMovementService _service;

        public AutomailService()
        {
            InitializeComponent(AppSettings.ServiceName);
        }

        /// <summary>
        /// On service start method.
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
            _service = new CreditMovementService();
        }

        protected override void OnStop()
        {
            timer.Stop();
            timer = null;
            _service = null;
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
                TaskInformation task = new TaskInformation();
                bool hasAlreadyRun = false;
                TaskInformation taskReadyToRun = _service.GetTaskReadyToRun();
                GetCreditMovementCriteria criteria = new GetCreditMovementCriteria
                {
                    // Amount = AppSettings.Amount
                };

                if (taskReadyToRun != null)
                {
                    DateTime taskRunningDay = Convert.ToDateTime(taskReadyToRun.CreatedDate);
                    hasAlreadyRun = DateTime.Now.Day == taskRunningDay.Day;
                }

                if (!AppSettings.OnProcessing && !hasAlreadyRun)
                {
                    TimeSpan executeTime = new TimeSpan(AppSettings.RunAtHours, AppSettings.RunAtMinutes, 0);
                    var currentTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                    int compareResult = TimeSpan.Compare(currentTime, executeTime);

                    if (compareResult >= 0)
                    {
                        task.TaskType = TaskType.Daily;
                        _service.InsertTask(task);
                        ////criteria.FromDate = criteria.ToDate = DateTime.Now.AddDays(-1);
                        _service.ProcessToSendEmail(criteria);
                    }
                }

                if (AppSettings.ForceRunning && !AppSettings.ForceRunningOnProcessing)
                {
                    AppSettings.ForceRunningOnProcessing = true;
                    task.TaskType = TaskType.Urgent;
                    _service.InsertTask(task);
                    ////criteria.FromDate = AppSettings.ForceRunningFromDate;
                    ////criteria.ToDate = AppSettings.ForceRunningToDate;
                    _service.ProcessToSendEmail(criteria);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }
    }
}