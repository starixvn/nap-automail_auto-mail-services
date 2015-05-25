// -----------------------------------------------------------------------
// <copyright file="ISMAReportService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.SMAReport.Business.Service
{
    using Fanex.SMAReport.Business.Model;
    using Fanex.MailDelivery.Client.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISMAReportService
    {
        Mail SMAReport();
    }
}