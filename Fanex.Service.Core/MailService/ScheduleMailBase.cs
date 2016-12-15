namespace Fanex.Service.Core.MailService
{
    using Fanex.MailDelivery.Client.Jobs;
    using MailDelivery.Core.Models;
    using Quartz;

    public abstract class ScheduleMailBase : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var jobResult = SendMailToVipGroup(schedule, jobContext);
            SendMailToNormalGroup();

            return jobResult;
        }

        public abstract JobMessage SendMailToVipGroup(Schedule schedule, IJobExecutionContext jobContext);

        public virtual void SendMailToNormalGroup()
        {
        }
    }
}