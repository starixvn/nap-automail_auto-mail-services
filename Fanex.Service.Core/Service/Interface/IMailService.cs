// -----------------------------------------------------------------------
// <copyright file="IWinLossByProductsMailService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business.Services
{
    using System.Collections.Generic;
    using Fanex.MailDelivery.Client.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMailService
    {
        Mail CreateMail(string subject, string title, string emailContent, List<MailAttachment> listAttachment, string functionName);

        void SendMailByFunction(Mail mail);
    }
}