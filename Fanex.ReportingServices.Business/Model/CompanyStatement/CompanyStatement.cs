// -----------------------------------------------------------------------
// <copyright file="CompanyStatement.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Model.CompanyStatement
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CompanyStatement
    {
        public CompanyStatement()
        {
            ReportRows = new List<CompanyStatement>();
        }

        public string Date { get; set; }

        public string Type { get; set; }

        public decimal Total { get; set; }

        public decimal AccumulatedTotal { get; set; }

        public List<CompanyStatement> ReportRows { get; set; }
    }
}