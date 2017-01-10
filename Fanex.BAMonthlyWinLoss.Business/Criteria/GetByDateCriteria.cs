namespace Fanex.NAPReport.BAMonthlyWinLoss.Business.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class GetByDateCriteria : CriteriaBase
    {
        public GetByDateCriteria()
        {
            if (AppSettings.IsTest)
            {
                FromDate = AppSettings.FromDate.ToShortDateString();
                ToDate = AppSettings.ToDate.ToShortDateString();
            }
            else
            {
                var date = DateTime.Today;
                if (date.Month == 1)
                {
                    date = date.AddMonths(-1);
                }

                FromDate = date.ToString("MM/01/yyyy");
                ToDate = $"{date.Month}/{DateTime.DaysInMonth(date.Year, date.Month)}/{date.Year}";
            }
        }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_Rpt_BALicWinlossMonthly";
        }
    }
}