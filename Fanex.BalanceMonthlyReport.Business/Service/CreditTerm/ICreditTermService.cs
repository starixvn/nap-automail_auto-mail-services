namespace Fanex.NAPReport.BalanceMonthlyReport.Business.Service.CreditTerm
{
    using Fanex.NAPReport.BalanceMonthlyReport.Business.Model;

    public interface ICreditTermService
    {
        string DirectoryOfFileName { get; set; }

        string FileName { get; set; }        

        void ExportExcelCreditTerm();
    }
}
