namespace Fanex.NAPReport.BalanceReport
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Overview Criteria.
    /// </summary>
    public class SuperBalanceReportCriteria : CriteriaBase
    {
        public string Keyword { get; set; }

        public string FirstChar { get; set; }

        public int Currency { get; set; }

        public bool IsViewTotalOnly { get; set; }

        public double Total { get; set; }

        /// <summary>
        /// Get SP name.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "Acc_Rpt_Balance_Super_Yesterday";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}