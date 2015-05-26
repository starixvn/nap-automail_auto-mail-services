// -----------------------------------------------------------------------
// <copyright file="UserInfo.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.RngCasino.Business.Entities
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UserInfo
    {
        public string Member { get; set; }

        public string Agency { get; set; }

        public int Currency { get; set; }

        public string CurrencyName { get; set; }

        public decimal WinlossBaseCurrency { get; set; }

        public decimal Winloss { get; set; }
    }
}
