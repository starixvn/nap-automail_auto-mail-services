namespace Fanex.NAPReport.BalanceMonthlyReport.Business
{
    using System;
    using Fanex.Data.Repository;

    public class BalanceReportBaseCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public DateTime Date { get; set; }

        [SpParam(Ignored = true)]
        public string Name { get; set; }

        [SpParam(Ignored = true)]
        public string Level { get; set; }

        [SpParam(Ignored = true)]
        public string DataDirectory
        {
            get;
            set;
        }

        [SpParam(Ignored = true)]
        public string DataDirectoryWithYear
        {
            get;
            set;
        }

        [SpParam(Ignored = true)]
        public string ExcelPasswordProtected
        {
            get;
            set;
        }

        [SpParam(Ignored = true)]
        public int FunctionId
        {
            get;
            set;
        }
    }
}