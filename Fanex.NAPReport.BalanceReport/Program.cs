using System;
using Fanex.NAPReport.BalanceReport.Bussiness;
using Fanex.Data.Configuration;
using Fanex.NAPReport.BalanceReport.Model;
using Fanex.NAPReport.BalanceReport.Core;

namespace Fanex.NAPReport.BalanceReport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DbSettingProviderManager.Current.Start();
                var report = new Report
                {
                    Name = "Credir Term",
                    Date = DateTime.Now
                };

                var subject = Utilities.LoadSubject(report);
                var body = Utilities.LoadBody(report);
                (new CreditTermManager()).ExportExcel();
                //(new SuperBalanceManager()).SendMail();
                //(new DirectBalanceManager()).SendMail();
            }
            catch (Exception ex)
            {
                Helper.Logger.WriteLog(ex.Message);
                Helper.Logger.WriteLog(ex.StackTrace);
            }
        }
    }
}
