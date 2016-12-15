// -----------------------------------------------------------------------
// <copyright file="RacingWinLoss.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.HorseRacingReport.Business.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RacingWinLoss
    {
        public RacingWinLoss()
        {
            ReportRows = new List<RacingWinLoss>();
        }

        public int PartnerId { get; set; }

        public string PartnerName { get; set; }

        public int PoolCurrency { get; set; }

        public string CurrencyName { get; set; }

        public int CurrencyId { get; set; }

        public int Race { get; set; }

        public decimal WinTOAgency { get; set; }

        public decimal PlaceTOAgency { get; set; }

        public decimal WinPlaceTOAgency { get; set; }

        public decimal TotalTOAgency { get; set; }

        public decimal CommIncomeAgency { get; set; }

        public decimal PTWinLossAgency { get; set; }

        public decimal WinTurnoverIBC { get; set; }

        public decimal PlaceTurnoverIBC { get; set; }

        public decimal WinPlaceTurnoverIBC { get; set; }

        public decimal TotalTurnoverIBC { get; set; }

        public decimal CommIncomeIBC { get; set; }

        public decimal ToteFeeIBC { get; set; }

        public decimal CommExpenseIBC { get; set; }

        public decimal PTWinLossIBC { get; set; }

        public decimal NetIncomeIBC { get; set; }

        public decimal WinTOPartner { get; set; }

        public decimal PlaceTOPartner { get; set; }

        public decimal WinPlaceTOPartner { get; set; }

        public decimal TotalTOPartner { get; set; }

        public decimal DividendWonPartner { get; set; }

        public decimal CommExpensePartner { get; set; }

        public decimal ToteFeePartner { get; set; }

        public decimal AmountDuePartner { get; set; }

        public List<RacingWinLoss> ReportRows { get; set; }

        public RacingWinLoss Total { get; set; }

        /// <summary>
        /// Calculates the total.
        /// </summary>
        /// <param name="racingWinLoss">The racing win loss.</param>
        public void CalculateTotal(RacingWinLoss racingWinLoss)
        {
            Race += racingWinLoss.Race;
            WinTOAgency += racingWinLoss.WinTOAgency;
            PlaceTOAgency += racingWinLoss.PlaceTOAgency;
            WinPlaceTOAgency += racingWinLoss.WinPlaceTOAgency;
            TotalTOAgency += racingWinLoss.TotalTOAgency;
            CommIncomeAgency += racingWinLoss.CommIncomeAgency;
            PTWinLossAgency += racingWinLoss.PTWinLossAgency;
            WinTurnoverIBC += racingWinLoss.WinTurnoverIBC;
            PlaceTurnoverIBC += racingWinLoss.PlaceTurnoverIBC;
            WinPlaceTurnoverIBC += racingWinLoss.WinPlaceTurnoverIBC;
            TotalTurnoverIBC += racingWinLoss.TotalTurnoverIBC;
            CommIncomeIBC += racingWinLoss.CommIncomeIBC;
            ToteFeeIBC += racingWinLoss.ToteFeeIBC;
            CommExpenseIBC += racingWinLoss.CommExpenseIBC;
            PTWinLossIBC += racingWinLoss.PTWinLossIBC;
            NetIncomeIBC += racingWinLoss.NetIncomeIBC;
            WinTOPartner += racingWinLoss.WinTOPartner;
            PlaceTOPartner += racingWinLoss.PlaceTOPartner;
            WinPlaceTOPartner += racingWinLoss.WinPlaceTOPartner;
            TotalTOPartner += racingWinLoss.TotalTOPartner;
            DividendWonPartner += racingWinLoss.DividendWonPartner;
            CommExpensePartner += racingWinLoss.CommExpensePartner;
            ToteFeePartner += racingWinLoss.ToteFeePartner;
            AmountDuePartner += racingWinLoss.AmountDuePartner;
        }
    }
}
