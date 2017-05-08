namespace Fanex.NAPReport.HorseTransaction3rd.Business.Criteria
{
    using System;
    using System.Collections;
    using Data;
    using Data.Repository;

    public class GetRacingWinLossCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public DateTime WinLostDate { get; set; }

        [SpParam(Ignored = true)]
        public int ReportType { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_Rpt_WinlostRacing_Tote";
        }

        public override Hashtable GetSpParams()
        {
            var parameterList = new Hashtable();

            parameterList.Add("winlostdate", this.WinLostDate.ToShortDateString());
            parameterList.Add("reporttype", this.ReportType);

            return parameterList;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}