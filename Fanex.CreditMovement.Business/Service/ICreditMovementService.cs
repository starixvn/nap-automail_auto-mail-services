// -----------------------------------------------------------------------
// <copyright file="ICreditMovementService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CreditMovement.Business.Service
{
    using Fanex.CreditMovement.Business.Criteria;
    using Fanex.CreditMovement.Business.Model;
    using Fanex.MailDelivery.Client.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICreditMovementService
    {
        Mail ProcessToSendEmail(GetCreditMovementCriteria criteria);
    }
}