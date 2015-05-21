namespace Fanex.BAMonthlyWinLoss.Business.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class GetByDateCriteria : CriteriaBase
    {
        public GetByDateCriteria(DateTime date)
        {
            FromDate = date.Month.ToString() + "/1/" + date.Year.ToString();
            ToDate = date.ToShortDateString();
        }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_Rpt_BALicWinlossMonthly";
        }
    }
}
