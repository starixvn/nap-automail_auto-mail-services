namespace Fanex.DW.AutoMailReport.Service.Jobs.DW
{
    using Fanex.DW.DailyBettypeWith3rdProducts.Business.Service;
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Quartz;

    public class DailyBettypeWith3rdProducts : JobBase
    {
        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            IMailingService job = new MailingService();
            var mail = job.ExportBettypeWith3rdProducts(true, schedule.Name);

            return new JobResultMessage()
            {
                JobStatus = mail.JobStatus,
                Mail = mail.Mail,
                Schedule = schedule,
            };
        }
    }
}