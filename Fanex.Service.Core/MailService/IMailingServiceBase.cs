// -----------------------------------------------------------------------
// <copyright file="IMailingService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.MailService
{
    using Fanex.MailDelivery.Client.Models;
    using Fanex.Service.Core.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMailingServiceBase
    {
        MailResult SendMailToVIPGroup();

        void SendMailToNormalGroup();
    }
}
