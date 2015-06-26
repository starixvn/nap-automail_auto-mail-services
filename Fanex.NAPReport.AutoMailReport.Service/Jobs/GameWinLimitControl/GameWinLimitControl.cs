// -----------------------------------------------------------------------
// <copyright file="GameWinLimitControl.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.GameWinLimitControl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Service.Core.MailService;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Quartz;
    using Fanex.NAPReport.GameWinLimitControl.Business.Services;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.NAPReport.GameWinLimitControl.Business;
    using Fanex.MailDelivery.Client.Handlers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GameWinLimitControl : ScheduleMailBase
    {
        private IMailingService service;
        public GameWinLimitControl()
        {
            service = new MailingService();
        }

        public override JobResultMessage SendMailToVipGroup(Schedule schedule, IJobExecutionContext jobContext)
        {
            var mailResult = service.SendMailToVIPGroup();
            
            return new JobResultMessage()
            {
                JobStatus = mailResult.JobStatus,
                Mail = mailResult.Mail,
                Schedule = schedule,
            };
        }
    }
}
