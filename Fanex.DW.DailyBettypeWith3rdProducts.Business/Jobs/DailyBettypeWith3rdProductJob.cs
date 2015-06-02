// -----------------------------------------------------------------------
// <copyright file="DailyBettypeWith3rdProductJob.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Fanex.DW.DailyBettypeWith3rdProducts.Business.Model;
using Fanex.DW.DailyBettypeWith3rdProducts.Business.Resource;
using Fanex.DW.DailyBettypeWith3rdProducts.Business.Service;
using Fanex.MailDelivery.Client.Enumerations;
using Fanex.MailDelivery.Client.Messages;
using Fanex.MailDelivery.Client.Models;

namespace Fanex.DW.DailyBettypeWith3rdProducts.Business.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DailyBettypeWith3rdProductJob : IDailyBettypeWith3rdProductJob
    {

        public Mail ProcessExportDailyBettypeWith3rdProducts(bool IsDaily, string Subject)
        {
            MailInfo info = new MailInfo(IsDaily);
            DailyBettypeWith3rdProductsService service = new DailyBettypeWith3rdProductsService();
            service.GenerateReport(false, info.ExportDataDirectory);

            string subject = Subject + " - " + DateTime.Now.ToString("yyyy-MM-dd");

            Mail mail = new Mail()
            {
                Subject = subject,
                Title = subject,
                Body = info.DailyBettype3rdProductMailBody
            };

            mail.Attachments = new List<MailAttachment>();
            mail.Attachments.Add(new MailAttachment(@service.GetFileName, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));
            service.GenerateReport(true, info.ExportDataDirectory);
            mail.Attachments.Add(new MailAttachment(@service.GetFileName, MailDelivery.Client.Enumerations.FileExtension.MS_EXCEL));

            return mail;
        }
    }
}
