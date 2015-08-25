namespace Fanex.NAPReport.RngCasino.Business.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class CustomerWinLossCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "fromdate")]
        public DateTime FromDate { get; set; }

        [SpParam(SqlParamName = "amount")]
        public decimal WinLoss { get; set; }

        [SpParam(SqlParamName = "todate")]
        public DateTime ToDate { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "RNGCasinoCustomerWinLoss";
        }
    }
}
