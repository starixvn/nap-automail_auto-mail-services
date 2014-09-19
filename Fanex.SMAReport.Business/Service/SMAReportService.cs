// -----------------------------------------------------------------------
// <copyright file="ResetWinningService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.SMAReport.Business.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.SMAReport.Business.Repositories;
    using Fanex.SMAReport.Business.Model;
    using Fanex.SMAReport.Business.Enums;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SMAReportService : ISMAReportService
    {
        ISMAReportRepository _repository = null;
        public SMAReportService()
        {
            _repository = new SMAReportRepository();
        }


        public void ProcessResetWinning()
        {
            try
            {
                SMAReportModel model = _repository.GetCurrentTask();
                if (model == null)
                    return;

                bool isSentMailSuccess = false;
                AppSettings.Id = model.Id;

                //Update task status from Running to Processing
                _repository.UpdateTaskStatus(AppSettings.Id, Status.Processing);

                //Call SP to Reset Winning
                Result result = _repository.ResetWinning(AppSettings.Id);
                if (result.ErrCode > 0)
                {
                    isSentMailSuccess = Mailing.ResetWinningEmailNotification(model, result);
                    _repository.UpdateTaskStatus(AppSettings.Id, Status.ResetFailed);
                }
                else
                {
                    isSentMailSuccess = Mailing.ResetWinningSuccess(model);
                    _repository.UpdateTaskStatus(AppSettings.Id, Status.ResetSuccessful);
                    _repository.MoveTaskToArchive(AppSettings.Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int InsertTask(SMAReportModel task)
        {
            int Id = _repository.InsertTask(task);
            return Id;
        }


        public Status GetSatatusByTaskId(int Id)
        {
            Status status = _repository.GetSatatusByTaskId(Id);
            return status;
        }
    }
}
