// -----------------------------------------------------------------------
// <copyright file="WeeklyBettypeWith3rdProducts.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------


using Fanex.DW.DailyBettypeWith3rdProducts.Business.Model;

namespace Fanex.DW.AutoMailReport.Service.Jobs.DW
{
    using System;
    using System.Collections.Generic;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Quartz;
    using Fanex.DW.DailyBettypeWith3rdProducts.Business.Service;
    using Fanex.DW.DailyBettypeWith3rdProducts.Business.Resource;
    using DailyBettypeWith3rdProductsSetting = Fanex.DW.DailyBettypeWith3rdProducts.Business;
    using Fanex.DW.DailyBettypeWith3rdProducts.Business.Jobs;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WeeklyBettypeWith3rdProducts : JobBase
    {
        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            IDailyBettypeWith3rdProductJob job = new DailyBettypeWith3rdProductJob();
            var mail = job.ProcessExportDailyBettypeWith3rdProducts(false, schedule.Name);
            
            return new JobResultMessage()
            {
                JobStatus = JobStatus.Success,
                Mail = mail,
                Schedule = schedule,                
            };
        }
    }
}
