using System;
using Fanex.NAPReport.BalanceReport.Bussiness;
using Fanex.Data.Configuration;

namespace Fanex.NAPReport.BalanceReport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DbSettingProviderManager.Current.Start();

                (new CreditTermManager()).ExportExcel();
                (new SuperBalanceManager()).ExportExcel();
                (new DirectBalanceManager()).ExportExcel();
            }
            catch (Exception ex)
            {
                Helper.Logger.WriteLog(ex.Message);
                Helper.Logger.WriteLog(ex.StackTrace);
            }
        }
    }
}
