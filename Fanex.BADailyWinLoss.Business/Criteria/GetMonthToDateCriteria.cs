namespace Fanex.NAPReport.BADailyWinLoss.Business.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetMonthToDateCriteria : CriteriaBase
    {
        public GetMonthToDateCriteria(DateTime date)
        {
            FromDate = date.Month.ToString() + "/1/" + date.Year.ToString();
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
