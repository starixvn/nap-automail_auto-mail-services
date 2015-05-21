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
                    Name = "Direct Balance",
                    Date = DateTime.Now,
                    IsDirectReport = true
                };
                // (new CreditTermManager()).ExportExcel(report);

                var subject = Utilities.LoadSubject(report);
                var body = Utilities.LoadBody(report);

                // Report Super.
                //var superCriteria = new SuperBalanceReportCriteria
                //{
                //    Date = DateTime.Now,
                //    Name = "Super Balance",
                //    Level = "Super"
                //};
                //(new BalanceManager(superCriteria)).ExportExcel();

                //// Report Master.
                //var masterCriteria = new DirectBalanceReportMasterCriteria
                //{
                //    Date = DateTime.Now,
                //    Name = "Direct Master",
                //    Level = "Master"
                //};
                //(new BalanceManager(masterCriteria)).ExportExcel();

                //// Report Agent.
                //var agentCriteria = new DirectBalanceReportAgentCriteria
                //{
                //    Date = DateTime.Now,
                //    Name = "Direct Agent",
                //    Level = "Agent"
                //};
                //(new BalanceManager(agentCriteria)).ExportExcel();

                //// Report Member.
                //var memberCriteria = new DirectBalanceReportMemberCriteria
                //{
                //    Date = DateTime.Now,
                //    Name = "Direct Member",
                //    Level = "Member"
                //};
                //(new BalanceManager(memberCriteria)).ExportExcel();
            }
            catch (Exception ex)
            {
                Helper.Logger.WriteLog(ex.Message);
                Helper.Logger.WriteLog(ex.StackTrace);
            }
        }
    }
}
