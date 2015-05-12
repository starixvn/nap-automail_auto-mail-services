// -----------------------------------------------------------------------
// <copyright file="WinLossByProducts.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.AutoMailReport.Service.Jobs.NAPReport
{
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Quartz;
using Fanex.WinLossByProducts.Business.Criteria;
    using System;
    using Fanex.MailDelivery.Client.Enumerations;
    using System.Collections.Generic;
    using Fanex.WinLossByProducts.Business.Services.Interface;
    using Fanex.WinLossByProducts.Business.Services.Implement;
    using Fanex.WinLossByProducts.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLossByProductsJob : JobBase
    {
        IWinLossByProductsService _winLossByProductsService;
        IWinLossByProductsExportDataService _exportWinLossByProductsDataService;

        public WinLossByProductsJob()
        {
            _winLossByProductsService = new WinLossByProductsService();
            _exportWinLossByProductsDataService = new WinLossByProductsExportDataService();
        }

        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            GetWinLossByProductsCriteria criteria = WinLossByProductsCriteria();
            WinLossByProducts winLossByProducts = _winLossByProductsService.GetWinLossByProductsData(criteria);

            string reportPath = _exportWinLossByProductsDataService.ExportReportToExcel(winLossByProducts, criteria);

            Mail mail = new Mail()
            {
                Subject = "subject",
                Title = "title",
                Body = "content"
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

        private GetWinLossByProductsCriteria WinLossByProductsCriteria()
        {
            GetWinLossByProductsCriteria criteria = new GetWinLossByProductsCriteria
            {
                WinLossDate = DateTime.Now.AddDays(-1)
            };

            return criteria;
        }
    }
}