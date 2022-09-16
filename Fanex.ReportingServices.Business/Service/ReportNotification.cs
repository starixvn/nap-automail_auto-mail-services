using Fanex.MailDelivery.Core.Models;
using Fanex.NAPReport.ReportingServices.Business.Service;
using Fanex.ReportingServices.Business.Model;
using System;

namespace Fanex.ReportingServices.Business.Service
{
    public interface ReportNotification
    {
        JobMessage Execute();
    }

    public class ReportNotificationImp : ReportNotification
    {
        public JobMessage Execute()
        {
            var result = new ExecuteResultModel();
            var log = new LogImp();
            JobMessage jobMessage = new JobMessage();
            try
            {
                result = SendNotification();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                log.Log(ex);
            }
            finally
            {
                jobMessage.Mail = result.Mail;
                jobMessage.JobStatus = result.Success ? JobStatus.Success : JobStatus.DataIsNotReady;
                jobMessage.TaskLog = new TaskLog
                {
                    LogTime = DateTime.Now,
                    Message = result.Message
                };
            }

            return jobMessage;
        }

        private ExecuteResultModel SendNotification()
        {
            var service = new MailingService();
            var mail = service.SendMailToVIPGroup();
            service.SendMailToNormalGroup();

            return new ExecuteResultModel
            {
                Mail = mail.Mail,
                Success = true,
                Message = string.Empty
            };
        }
    }
}