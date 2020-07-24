using Fanex.BalanceMonthlyReport.Business.Model;
using Fanex.MailDelivery.Core.Models;
using Fanex.NAPReport.BalanceMonthlyReport.Business.Service.Mailing;
using Fanex.Service.Core.Extensions;
using System;

namespace Fanex.BalanceMonthlyReport.Business.Service
{
    public interface CreditTermNotification
    {
        JobMessage Execute();
    }

    public class CreditTermNotificationImp : CreditTermNotification
    {
        public JobMessage Execute()
        {
            var result = new ExecuteResultModel();
            JobMessage jobMessage = new JobMessage();
            try
            {
                result = SendNotification();
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;
                LogExtensions.ExceptionHandler(e);
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
            var mail = service.SendMailCreditTermToVIPGroup();
            service.SendMailToCreditTermNormalGroup();

            return new ExecuteResultModel
            {
                Mail = mail,
                Success = true,
                Message = string.Empty
            };
        }
    }
}