// -----------------------------------------------------------------------
// <copyright file="AutomailService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.HorseRacingReport.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.ServiceProcess;
    using System.Timers;
    using Fanex.Data.Configuration;
    using Fanex.HorseRacingReport.Business;
    using Fanex.HorseRacingReport.Business.Criteria;
    using Fanex.HorseRacingReport.Business.Model;
    using Fanex.HorseRacingReport.Business.Repositories;
    using Fanex.Service.Core.Extensions;

    /// <summary>
    /// Auto mail service.
    /// </summary>
    public partial class AutomailService : ServiceBase
    {
        /// <summary>
        /// The timer.
        /// </summary>
        private static Timer timer;

        private static ProcessHorseRacingService service;

        private static TaskRepository taskRepository;

        public AutomailService()
        {
            InitializeComponent(AppSettings.ServiceName);
        }

        /// <summary>
        /// Handle event when service start.
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
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        private static void OnInterval(object source, ElapsedEventArgs e)
        {
            try
            {
                DateTime currentTime = DateTime.Now;
                bool isRunningDay = currentTime.DayOfWeek == DayOfWeek.Thursday || currentTime.DayOfWeek == DayOfWeek.Monday;
                if (!AppSettings.ForceRunning && !isRunningDay)
                {
                    return;
                }

                service = new ProcessHorseRacingService();
                taskRepository = new TaskRepository();
                TimeRangeCriteria timeRangeCriteria;

                TaskModel taskModel = taskRepository.GetTaskReadyRun();
                bool hasAlreadyRun = false;
                if (taskModel != null)
                {
                    DateTime taskRunningDay = Convert.ToDateTime(taskModel.CreatedDate);
                    hasAlreadyRun = DateTime.Now.Day == taskRunningDay.Day;
                }

                if (isRunningDay && !AppSettings.OnProcessing && !hasAlreadyRun)
                {
                    AppSettings.OnProcessing = true;
                    TimeSpan executeTime = new TimeSpan(AppSettings.RunAtHours, AppSettings.RunAtMinutes, 0);
                    var currentTimeByHoursAndMinutes = new TimeSpan(currentTime.Hour, currentTime.Minute, 0);
                    int compareResult = TimeSpan.Compare(currentTimeByHoursAndMinutes, executeTime);

                    if (compareResult >= 0)
                    {
                        taskRepository.InsertTask(taskModel);
                        timeRangeCriteria = service.GetTimeRangeCriteria(currentTime);
                        service.ProcessHorseRacingReportService(timeRangeCriteria);
                        AppSettings.OnProcessing = false;
                    }
                }

                if (AppSettings.ForceRunning && !AppSettings.ForceRunningOnProcessing)
                {
                    AppSettings.ForceRunningOnProcessing = true;
                    taskRepository.InsertTask(taskModel);
                    timeRangeCriteria = new TimeRangeCriteria();
                    timeRangeCriteria.FromDate = AppSettings.ForceRunningFromDate;
                    timeRangeCriteria.ToDate = AppSettings.ForceRunningToDate;
                    service.ProcessHorseRacingReportService(timeRangeCriteria);
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