using Fanex.Data.Repository;

namespace Fanex.NAPReport.GameWinLimitControl.Business.Criteria
{
    public class UdpMemberWinLimitCriteria : CriteriaBase
    {
        public string MemNotify { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_Notification_MemUpd";
        }

        public override bool IsValid()
        {
            return true;
        }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}
