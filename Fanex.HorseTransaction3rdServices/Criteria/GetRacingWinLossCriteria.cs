namespace Fanex.HorseTransaction3rdServices.Criteria
{
    using System;
    using System.Collections;
    using Fanex.NAP.Framework.Data;
    using Fanex.Service.Core.Service;

    public class GetRacingWinLossCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public DateTime WinLostDate { get; set; }
        
        [SpParam(Ignored = true)]
        public int ReportType { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_Rpt_WinlostRacing_Tote" + BaseService.GetStoreMappingSuffix(this.WinLostDate);
        }

        public override Hashtable GetSpParams()
        {
            var parameterList = new Hashtable();

            parameterList.Add("winlostdate", this.WinLostDate.ToShortDateString());
            parameterList.Add("reporttype", this.ReportType);

            return parameterList;
        }
    }
}
