namespace Fanex.NAPReport.SMALoss.Business.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class MailingListSelCriteria : CriteriaBase
    {
        public int FuncId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_MailingListSel";
        }

        public override bool IsValid()
        {
            return FuncId > 0;
        }
    }
}