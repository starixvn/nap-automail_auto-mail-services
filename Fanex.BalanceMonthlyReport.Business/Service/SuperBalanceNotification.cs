using Fanex.BalanceMonthlyReport.Business.Model;
using Fanex.MailDelivery.Core.Models;
using Fanex.NAPReport.BalanceMonthlyReport.Business.Service.Mailing;
using System;

namespace Fanex.BalanceMonthlyReport.Business.Service
{
    public interface SuperBalanceNotification
    {
        JobMessage Execute();
    }

    public class SuperBalanceNotificationImp : SuperBalanceNotification
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
                jobMessage.Mail = new Mail { Subject = string.Empty, Body = string.Empty };
                jobMessage.JobStatus = result.Success ? JobStatus.SuccessWithoutSendmail : JobStatus.DataIsNotReady;
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
            var mail = service.SendMailSuperBalanceToVIPGroup();
            service.SendMailToSuperBalanceNormalGroup();

            return new ExecuteResultModel
            {
                Mail = mail,
                Success = true,
                Message = string.Empty
            };
        }
    }
}