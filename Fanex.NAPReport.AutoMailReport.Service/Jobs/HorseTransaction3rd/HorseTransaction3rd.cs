namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.HorseTransaction3rd
{
    using System.Linq;
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Resource;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Service;
    using MailDelivery.Core.Models;
    using Quartz;

    public class HorseTransaction3rd : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
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

            return new JobMessage()
            {
                JobStatus = JobStatus.Success,
                Mail = mail,
                Schedule = schedule,
            };
        }
    }
}