// -----------------------------------------------------------------------
// <copyright file="DailyBettypeWith3rdProducts.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.DW.AutoMailReport.Service.Jobs.DW
{
    using System;
    using System.Collections.Generic;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Quartz;
    using Fanex.DW.DailyBettypeWith3rdProducts.Business.Service;
    using Fanex.DW.DailyBettypeWith3rdProducts.Business.Resource;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DailyBettypeWith3rdProducts : JobBase
    {
        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            DailyBettypeWith3rdProductsService service = new DailyBettypeWith3rdProductsService();
            service.GenerateReport(false);
            Mail mail = new Mail()
            {
                Subject = string.Format(Label.DailyBettype3rdProductMailSubject, DateTime.Now.ToString("yyyy-MM-dd")),
                Title = string.Format(Label.DailyBettype3rdProductMailSubject, DateTime.Now.ToString("yyyy-MM-dd")),
                Body = Label.DailyBettype3rdProductMailBody
            };

            mail.Attachments = new List<MailAttachment>();
            mail.Attachments.Add(new MailAttachment(@service.GetFileName, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));
            service.GenerateReport(true);
            mail.Attachments.Add(new MailAttachment(@service.GetFileName, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));
            return new JobResultMessage()
            {
                JobStatus = JobStatus.Success,
                Mail = mail,
                Schedule = schedule,
                Result = new ResultMessage
                {
                    ErrCode = 0,
                    ErrMsg = string.Empty
                }
            };
        }
    }
}
