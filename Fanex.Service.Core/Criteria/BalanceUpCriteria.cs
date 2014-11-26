// -----------------------------------------------------------------------
// <copyright file="BalanceUpCriteria.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BalanceUpCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Core_ReportService_GetBalanceUpDay";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}