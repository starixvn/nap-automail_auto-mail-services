namespace Fanex.NAP.BADailyWinLoss.Business.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class GetYearToDateCriteria : CriteriaBase
    {
        public GetYearToDateCriteria(DateTime date)
        {
            FromDate = "1/1/" + date.Year.ToString();
            ToDate = date.ToShortDateString();
        }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_Rpt_BALicWinloss";
        }
    }
}
