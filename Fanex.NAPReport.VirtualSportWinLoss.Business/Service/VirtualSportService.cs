// -----------------------------------------------------------------------
// <copyright file="VirtualSportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Franklin.Nguyen" date="2014.05.30">Created</change>
// </history>

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Service
{
    using System;
    using System.Data;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Common;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Enums;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Repositories.Implement;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Repositories.Interface;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Implement;
    using Fanex.NAPReport.VirtualSportWinLoss.Business.Service.Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class VirtualSportService
    {
        private IVirtualSportRepository _repository = null;
        private IExportExcelService _service;

        public VirtualSportService()
        {
            _repository = new VirtualSportRepository();
            _service = new ExportExcelService();
        }

        public void OnRunningDaily()
        {
            ProcessBusinessDaily();
        }

        /// <summary>
        /// Processes the business daily.
        /// </summary>
        /// <returns>Identify of task.</returns>
        private int ProcessBusinessDaily()
        {
            int id = 0;

            if (!AppSettings.OnProcessing)
            {
                try
                {
                    AppSettings.OnProcessing = true;

                    if (_repository.HasTaskAlreadyRunToday())
                    {
                        CheckNotCompleteTask();
                    }
                    else
                    {
                        id = _repository.Insert(TaskType.Daily);

                        bool isSendEmailSuccess = _service.ProcessDataToSendMail();
                        var status = isSendEmailSuccess ? Status.SentEmailSuccessful : Status.SentEmailFailure;
                        _repository.UpdateStatus(id, status);
                    }
                }
                catch (Exception ex)
                {
                    LogExtensions.ExceptionHandler(ex);
                }

                AppSettings.OnProcessing = false;
            }

            return id;
        }

        /// <summary>
        /// Checks the not complete task.
        /// </summary>
        private void CheckNotCompleteTask()
        {
            DataTable log = _repository.GetLogNotCompleteTask();
            if (log.Rows.Count > 0)
            {
                int id = Convert.ToInt32(log.Rows[0]["Id"]);
                DateTime updatedDate = Convert.ToDateTime(log.Rows[0]["UpdatedDate"]).AddMinutes(AppSettings.RunNextTimeInMinutes);

                var currentTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                TimeSpan executeTime = new TimeSpan(updatedDate.Hour, updatedDate.Minute, 0);

                bool isSendEmailSuccess = false;
                int compareResult = TimeSpan.Compare(currentTime, executeTime);
                if (compareResult >= 0)
                {
                    isSendEmailSuccess = _service.ProcessDataToSendMail();
                    var status = isSendEmailSuccess ? Status.SentEmailSuccessful : Status.SentEmailFailure;
                    _repository.UpdateStatus(id, status);
                }
            }
        }
    }
}