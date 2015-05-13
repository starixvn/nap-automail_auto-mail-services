namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.NAPReport
{
    using System;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.SMALoss.Business.Services;
    using Quartz;

    public class SMALoss : JobBase
    {
        private const string TemplateUrl = @"/Templates/EmailTemplate.cshtml";

        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var lossProcessService = new SMALossProcessService();
            lossProcessService.NotifyEmail(new Uri(TemplateUrl));
            return new JobResultMessage()
            {
                JobStatus = JobStatus.Success,
                //Mail = mail,
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