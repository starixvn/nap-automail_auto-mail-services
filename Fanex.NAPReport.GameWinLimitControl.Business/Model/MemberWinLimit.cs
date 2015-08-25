// -----------------------------------------------------------------------
// <copyright file="MemberWinLimit.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.GameWinLimitControl.Business.Model
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberWinLimit
    {
        public MemberWinLimit()
        {
            if (!AppSettings.IsCallFPSBlockMember)
            {
                BlockStatus = true;
            }
        }

        public int CustId { get; set; }

        public string Username { get; set; }

        public string Username2 { get; set; }

        public string SiteName { get; set; }

        public string Currency { get; set; }

        public double MaxWinLimit { get; set; }

        public double TotalWinAmt { get; set; }

        public double Percentage { get; set; }

        public bool BlockStatus { get; set; }
    }
}
