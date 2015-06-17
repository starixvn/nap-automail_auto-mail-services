using Fanex.NAPReport.BalanceMonthlyReport.Business.Model;

namespace Fanex.NAPReport.BalanceMonthlyReport.Business.Service.CreditTerm
{
    public interface ICreditTermService
    {
        string DirectoryOfFileName { get; set; }

        string FileName { get; set; }        

        void ExportExcelCreditTerm();
    }
}
