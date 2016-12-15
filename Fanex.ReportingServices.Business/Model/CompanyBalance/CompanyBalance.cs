// -----------------------------------------------------------------------
// <copyright file="CompanyBalance.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Model.CompanyBalance
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CompanyBalance
    {
        public CompanyBalance()
        {
            ReportRows = new List<CompanyBalance>();
        }

        public string CurrencyName { get; set; }

        public decimal TotalAmountLocal { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal TodayRate { get; set; }

        public decimal YesterdayRate { get; set; }

        public List<CompanyBalance> ReportRows { get; set; }

        public CompanyBalance GrandTotal { get; set; }

        public void AddToGrandTotal(CompanyBalance balance)
        {
            this.GrandTotal.TotalAmount += balance.TotalAmount;
        }
    }
}