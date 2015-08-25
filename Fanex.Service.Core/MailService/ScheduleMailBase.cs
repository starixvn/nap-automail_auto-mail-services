namespace Fanex.Service.Core.MailService
{
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Quartz;

    public abstract class ScheduleMailBase : JobBase
    {
        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var jobResult = SendMailToVipGroup(schedule, jobContext);
            SendMailToNormalGroup();

            return jobResult;
        }

        public abstract JobResultMessage SendMailToVipGroup(Schedule schedule, IJobExecutionContext jobContext);

        public virtual void SendMailToNormalGroup()
        {
        }
    }
}