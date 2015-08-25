// -----------------------------------------------------------------------
// <copyright file="ICreditMovementRepository.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CreditMovement.Business.Repositories
{
    using System.Collections.Generic;
    using Fanex.CreditMovement.Business.Criteria;
    using Fanex.CreditMovement.Business.Model;
    using Fanex.Service.Core.Enums;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICreditMovementRepository
    {
        List<CreditMovement> GetReportData(GetCreditMovementCriteria criteria);

        List<string> GetSiteId();
    }
}