namespace Fanex.NAPReport.BalanceReport
{
    using Fanex.Data.Repository;
    using System;

    /// <summary>
    /// Overview Criteria.
    /// </summary>
    public class BalanceReportBaseCriteria : CriteriaBase
    {
        [SpParam(Ignored=true)]
        public DateTime Date { get; set; }

        [SpParam(Ignored = true)]
        public string Name { get; set; }

        [SpParam(Ignored = true)]
        public string Level { get; set; }
    }
}