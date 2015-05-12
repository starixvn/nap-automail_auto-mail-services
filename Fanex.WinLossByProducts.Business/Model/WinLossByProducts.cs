// -----------------------------------------------------------------------
// <copyright file="WinLossByProducts.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.WinLossByProducts.Business.Model
{
    using System;
    using System.Collections.Generic;
    using Fanex.Service.Core.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLossByProducts
    {
        public WinLossByProducts()
        {
            Sportbook = new List<WinLossByProducts>();
            ThirdPartySport = new List<WinLossByProducts>();
        }

        public string Date
        {
            get
            {
                return DateTime.Now.AddDays(-1).ToShortFormatedDate();
            }
        }

        public string GroupName { get; set; }

        public string ProductName { get; set; }

        public decimal WinLoss { get; set; }

        public decimal MonthToDateWinLoss { get; set; }

        public bool IsEnoughData { get; set; }

        public List<WinLossByProducts> Sportbook { get; set; }

        public List<WinLossByProducts> ThirdPartySport { get; set; }

        public WinLossByProducts SportbookTotal { get; set; }

        public WinLossByProducts Total { get; set; }

        public void CalculateTotal(WinLossByProducts winLossByProducts)
        {
            this.WinLoss += winLossByProducts.WinLoss;
            this.MonthToDateWinLoss += winLossByProducts.MonthToDateWinLoss;
        }
    }
}