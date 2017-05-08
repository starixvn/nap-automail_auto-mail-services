using System;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.NAPReport.HorseTransaction3rd.Business.Service
{
    public interface TransactionService
    {
        JobMessage Execute();
    }

    public class TransactionServiceImp : TransactionService
    {
        public JobMessage Execute()
        {
            var jobMessage = new JobMessage();
            var logService = new LogServiceImp();
            var ftp = new GetFilesTransactionManager();
            var success = false;
            var message = string.Empty;
            try
            {
                ftp.GetFilesTransaction();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                message = ex.Message;
                logService.Log(ex);
            }
            finally
            {
                var content = string.Join("<br/>", ftp.MailList);
                var mail = new Mail()
                {
                    Subject = "Getting the files via Ftp Server",
                    Title = "Getting the files via Ftp Server",
                    Body = content
                };

                jobMessage = new JobMessage
                {
                    Mail = mail,
                    JobStatus = success == true
                    ? JobStatus.Success
                    : JobStatus.DataIsNotReady,
                    TaskLog = new TaskLog
                    {
                        Message = message
                    }
                };
            }

            return jobMessage;
        }
    }
}