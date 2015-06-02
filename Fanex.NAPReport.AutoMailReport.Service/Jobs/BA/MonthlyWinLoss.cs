namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.BA
{
    using System;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.Service.Core.MailService;
    using Quartz;
    using Fanex.NAPReport.BAMonthlyWinLoss.Business.Service;
    using Fanex.MailDelivery.Client.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MonthlyWinLoss : ScheduleMailBase
    {
        private IMailingService service;

        public MonthlyWinLoss()
        {
            service = new MailingService();
        }

        public override JobResultMessage SendMailToVipGroup(Schedule schedule, IJobExecutionContext jobContext)
        {
            var mail = service.SendMailToVIPGroup();

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

        public override void SendMailToNormalGroup()
        {
            service.SendMailToNormalGroup();
        }
    }
}