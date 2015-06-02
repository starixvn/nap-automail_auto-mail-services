// -----------------------------------------------------------------------
// <copyright file="IMailService.cs" company="Microsoft">
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
    public interface IMailStationService
    {
        Mail CreateMailWithAttachedFile(MailInfo mailInfo);

        Mail CreateMailWithNotification(MailInfo mailInfo);

        void SendFunctionMailWithAttachedFile(int functionId, MailInfo mailInfo);

        void SendFunctionMailWithNotification(int functionId, MailInfo mailInfo);
    }
}
