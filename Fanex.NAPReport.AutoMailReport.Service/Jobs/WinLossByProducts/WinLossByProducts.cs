// -----------------------------------------------------------------------
// <copyright file="WinLossByProductsJob.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.WinLossByProducts
{
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.WinLossByProducts.Business.Services;
    using Fanex.Service.Core.MailService;
    using Quartz;
    using System;
    using Fanex.Service.Core.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLossByProducts : ScheduleMailBase
    {
        private MailingService service;

        public WinLossByProducts()
        {
            service = new MailingService();
        }

        public override JobResultMessage SendMailToVipGroup(Schedule schedule, IJobExecutionContext jobContext)
        {
            JobResultMessage result = new JobResultMessage();

            try
            {
                var mail = service.SendMailToVIPGroup();

                return new JobResultMessage()
                {
                    JobStatus = mail.JobStatus,
                    Mail = mail.Mail,
                    Schedule = schedule,
                };

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public override void SendMailToNormalGroup()
        {
            service.SendMailToNormalGroup();
        }
    }
}
