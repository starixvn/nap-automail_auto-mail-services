// -----------------------------------------------------------------------
// <copyright file="IMaillingService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.HorseRacingReport.Business.Service
{
    using MailDelivery.Core.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMailingService
    {
        Mail SendMailToVIPGroup();

        void SendMailToNormalGroup();
    }
}