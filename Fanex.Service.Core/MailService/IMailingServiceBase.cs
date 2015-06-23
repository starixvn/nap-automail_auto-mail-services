// -----------------------------------------------------------------------
// <copyright file="IMailingService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.MailService
{
    using Fanex.MailDelivery.Client.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMailingServiceBase
    {
        Mail SendMailToVIPGroup();

        void SendMailToNormalGroup();
    }
}
