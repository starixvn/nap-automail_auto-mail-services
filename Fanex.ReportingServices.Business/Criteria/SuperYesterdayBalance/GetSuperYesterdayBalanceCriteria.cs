// -----------------------------------------------------------------------
// <copyright file="GetSuperYesterdayBalanceCriteria.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Criteria.SuperYesterdayBalance
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetSuperYesterdayBalanceCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "Keyword")]
        public string Username
        {
            get
            {
                return string.Empty;
            }
        }

        [SpParam(SqlParamName = "FirstChar")]
        public string FirstLetter
        {
            get
            {
                return string.Empty;
            }
        }

        [SpParam(SqlParamName = "Currency")]
        public int? Currency
        {
            get
            {
                return null;
            }
        }

        [SpParam(SqlParamName = "IsViewTotalOnly")]
        public bool IsViewTotalOnly
        {
            get
            {
                return false;
            }
        }

        public override bool IsValid()
        {
            return true;
        }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "Balance_SuperBalance_SuperYesterday";
        }
    }
}