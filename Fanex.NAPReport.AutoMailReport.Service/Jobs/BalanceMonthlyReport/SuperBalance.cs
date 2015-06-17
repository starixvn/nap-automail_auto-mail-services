// -----------------------------------------------------------------------
// <copyright file="SuperBalance.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.BalanceMonthlyReport
{
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Service.Mailing;
    using Fanex.Service.Core.MailService;
    using Quartz;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SuperBalance : ScheduleMailBase
    {
        private IMailingService service;

        public SuperBalance()
        {
            service = new MailingService();
        }

        public override JobResultMessage SendMailToVipGroup(Schedule schedule, IJobExecutionContext jobContext)
        {
            var mail = service.SendMailSuperBalanceToVIPGroup();

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
            service.SendMailToSuperBalanceNormalGroup();
        }
    }
}
