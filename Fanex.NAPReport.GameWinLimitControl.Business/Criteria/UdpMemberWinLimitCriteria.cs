namespace Fanex.NAPReport.GameWinLimitControl.Business.Criteria
{
    using Fanex.Data.Repository;

    public class UdpMemberWinLimitCriteria : CriteriaBase
    {
        public string MemNotify { get; set; }        

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_Notification_MemUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
