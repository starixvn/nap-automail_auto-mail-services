namespace Fanex.NAPReport.BalanceMonthlyReport.Business.Service.Balance
{
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Model;
    public interface IBalanceService
    {
        string DirectoryOfFileName { get; }        

        string FileName { get; set; }        

        void ExportExcelBalance();
    }
}
