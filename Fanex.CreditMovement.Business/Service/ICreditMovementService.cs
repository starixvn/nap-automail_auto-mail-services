// -----------------------------------------------------------------------
// <copyright file="ICreditMovementService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CreditMovement.Business.Service
{
    using Fanex.CreditMovement.Business.Criteria;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICreditMovementService
    {
        string DirectoryOfFileName { get; set; }

        string FileName { get; set; }

        void ExportExcel(GetCreditMovementCriteria criteria);
    }
}