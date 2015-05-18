namespace Fanex.NAPReport.BalanceReport.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Customer Information.
    /// </summary>
    public class Report
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public bool IsDirectReport { get; set; }
    }
}