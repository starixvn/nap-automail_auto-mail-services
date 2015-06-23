// -----------------------------------------------------------------------
// <copyright file="WinLossByProductsJob.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.WinLossByProducts
{
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.WinLossByProducts.Business.Services;
    using Fanex.Service.Core.MailService;
    using Quartz;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLossByProducts : ScheduleMailBase
    {
        private MailingService service;

        public WinLossByProducts()
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
