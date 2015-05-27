// -----------------------------------------------------------------------
// <copyright file="IDailyBettypeWith3rdProductJob.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------



namespace Fanex.DW.DailyBettypeWith3rdProducts.Business.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IDailyBettypeWith3rdProductJob
    {
        Mail ProcessExportDailyBettypeWith3rdProducts(bool IsDaily);
    }
}
