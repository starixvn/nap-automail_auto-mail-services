// -----------------------------------------------------------------------
// <copyright file="GetCurrencyListCriteria.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Fanex.NAP.Framework.Data;

namespace Fanex.NAPReport.RngCasino.Business.Criteria
{
	/// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetCurrencyListCriteria : CriteriaBase
    {
        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "NAP_ExchangeCurrencySel";
        }
    }
}
