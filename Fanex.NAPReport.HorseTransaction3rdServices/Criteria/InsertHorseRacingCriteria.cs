namespace Fanex.NAPReport.HorseTransaction3rd.Business.Criteria
{
    using System.Collections;
    using Fanex.NAP.Framework.Data;

    public class InsertHorseRacingCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public Hashtable ParameterList { get; set; }       
        
        public override string GetSettingKey()
        {
            return "Acc_Rpt_Racing_Transaction3rdIns";
        }

        public override Hashtable GetSpParams()
        {            
            return ParameterList;
        }
    }
}
