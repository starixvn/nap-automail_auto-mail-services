using System;
using Fanex.Data.Repository;

namespace Fanex.ProductsWinLoss.Business.Criteria
{
    public class HouseNetIncomeEXDepositCriteria : CriteriaBase
    {
        public DateTime FromDate
        {
            get
            {
                var date = DateTime.Today.AddDays(-1);
                return Convert.ToDateTime($"{date:yyyy/MM/01}");
            }
        }

        public DateTime ToDate
        {
            get
            {
                return DateTime.Today.AddDays(-1);
            }
        }

        public override string GetSettingKey() => "Acc_Rpt_HouseNetIncome_EX_Deposit";

        public override bool IsValid() => true;
    }
}