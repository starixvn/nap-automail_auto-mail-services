// -----------------------------------------------------------------------
// <copyright file="IMailingService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.GameWinLimitControl.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.Service.Core.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMailingService
    {
        MailResult SendMailToVIPGroup();

        void SendMailToNormalGroup();
    }
}
