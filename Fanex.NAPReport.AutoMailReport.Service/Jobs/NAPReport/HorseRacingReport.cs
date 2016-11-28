// -----------------------------------------------------------------------
// <copyright file="HorseRacingReport.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.NAPReport
{
    using Fanex.NAPReport.HorseRacingReport.Business.Service;
    using Fanex.Service.Core.MailService;
    using MailDelivery.Core.Models;
    using Quartz;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HorseRacingReport : ScheduleMailBase
    {
        private IMailingService service;

        public HorseRacingReport()
        {
            service = new MailingService();
        }

        public override JobMessage SendMailToVipGroup(Schedule schedule, IJobExecutionContext jobContext)
        {
            var mail = service.SendMailToVIPGroup();

            return new JobMessage()
            {
                JobStatus = JobStatus.Success,
                Mail = mail,
                Schedule = schedule,
            };
        }

        public override void SendMailToNormalGroup()
        {
            service.SendMailToNormalGroup();
        }
    }
}