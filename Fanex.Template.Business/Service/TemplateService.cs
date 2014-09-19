// -----------------------------------------------------------------------
// <copyright file="ResetWinningService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Template.Business.Service
{
    using System;
    using Fanex.Template.Business.Repositories;
    using Fanex.Template.Business.Business;
    using Fanex.Template.Business.Model;
    using Fanex.Template.Business.Enums;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TemplateService : ITemplateService
    {
        ITemplateRepository _repository = null;
        public TemplateService()
        {
            _repository = new TemplateRepository();
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
