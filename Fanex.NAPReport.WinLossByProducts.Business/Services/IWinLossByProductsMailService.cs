// -----------------------------------------------------------------------
// <copyright file="IWinLossByProductsMailService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business.Services
{
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAPReport.WinLossByProducts.Business.Criteria;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IWinLossByProductsMailService
    {
        Mail CreateMail(string emailContent, string attachReportPath, GetWinLossByProductsCriteria criteria);

        void SendMailByFunction(Mail mail);
    }
}