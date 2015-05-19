namespace Fanex.NAPReport.BalanceReport.Bussiness
{
    using Fanex.NAPReport.BalanceReport.Model;
    public interface IBalanceManager
    {
        string ReportPath { get; }

        void ExportExcel();
    }
}
