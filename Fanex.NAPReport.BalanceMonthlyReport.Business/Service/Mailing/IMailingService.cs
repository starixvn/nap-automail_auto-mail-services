// -----------------------------------------------------------------------
// <copyright file="IMailingService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.BalanceMonthlyReport.Business.Service.Mailing
{
    using Fanex.MailDelivery.Client.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMailingService
    {
        Mail SendMailCreditTermToVIPGroup();        

        Mail SendMailSuperBalanceToVIPGroup();

        Mail SendMailDirectBalanceToVIPGroup();

        void SendMailToCreditTermNormalGroup();

        void SendMailToSuperBalanceNormalGroup();

        void SendMailToDirectBalanceNormalGroup();
    }
}
