using Fanex.MailDelivery.Client.Enumerations;
using Fanex.MailDelivery.Client.Jobs;
using Fanex.MailDelivery.Client.Messages;
using Fanex.MailDelivery.Client.Models;
using Quartz;

namespace Fanex.NegativeBalanceByMatch.Service.Jobs
{
    public class NegativeBalanceByMatchJob: JobBase
    {
        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            Mail mail = new Mail()
            {
                Subject = "G Subject",
                Title = "G Title",
                Body = "G Body"
            };

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
