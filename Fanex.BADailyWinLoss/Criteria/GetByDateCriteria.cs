namespace Fanex.BADailyWinLoss.Business.Criteria
{
    using System;
    using System.Collections;
    using Fanex.NAP.Framework.Data;

    public class GetByDateCriteria : CriteriaBase
    {
        [SpParam(Ignored = false)]
        public DateTime Date { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_Rpt_BALicWinlossDaily";
        }

        public override Hashtable GetSpParams()
        {
            var parameters = new Hashtable();
            parameters.Add("date", Date.ToShortDateString());

            return parameters;
        }
    }
}
