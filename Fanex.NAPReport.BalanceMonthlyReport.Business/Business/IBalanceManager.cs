namespace Fanex.NAPReport.BalanceMonthlyReport.Business.Bussiness
{
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Model;
    public interface IBalanceManager
    {
        string ReportPath { get; }

        void ExportExcel();
    }
}
