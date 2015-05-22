namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.HorseTransaction3rd
{
    using System.Linq;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Resource;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Service;
    using Quartz;

    public class HorseTransaction3rd : JobBase
    {
        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var ftp = new GetFilesTransactionManager();
            ftp.GetFilesTransaction();
            var content = string.Join("<br/>", ftp.MailList.Select(i => i.Value));

            var mail = new Mail()
            {
                Subject = Resource.SubjectOfMail,
                Title = Resource.SubjectOfMail,
                Body = content
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
