// -----------------------------------------------------------------------
// <copyright file="WinLossDetailByBettype.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="David.Tran" date="2014.05.13">Created</change>
// </history>

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Detail
{
    using System;
    using Fanex.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLossDetailByBettype
    {
        public DateTime WinLossDate { get; set; }

        public int SportID { get; set; }

        public string SportName { get; set; }

        public int BetCount { get; set; }

        public int BettypeID { get; set; }

        public string BettypeName { get; set; }

        public double TurnOver { get; set; }

        public double WinLoss { get; set; }

        public double Margin
        {
            get
            {
                if (this.TurnOver.Equals(0))
                {
                    return 0;
                }

                var result = Converter.To<double>(this.WinLoss / this.TurnOver);
                return result;
            }
        }
    }
}