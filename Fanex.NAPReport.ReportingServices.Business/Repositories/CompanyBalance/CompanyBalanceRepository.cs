// -----------------------------------------------------------------------
// <copyright file="CompanyBalanceRepository.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Repositories.CompanyBalance
{
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAPReport.ReportingServices.Business.Criteria.CompanyBalance;
    using Fanex.NAPReport.ReportingServices.Business.Model.CompanyBalance;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CompanyBalanceRepository : ICompanyBalanceRepository
    {
        public CompanyBalance GetCompanyBalanceReport(GetCompanyBalanceCriteria criteria)
        {
            CompanyBalance balance = new CompanyBalance();
            IDataReader reader;
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                reader = objectDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    balance.ReportRows.Add(new CompanyBalance
                    {
                        CurrencyName = Converter.ToString2(reader["CurrencyName"]),
                        TotalAmountLocal = Converter.ToDecimal(reader["TotalamtLocal"]),
                        TotalAmount = Converter.ToDecimal(reader["TotalAmt"]),
                        TodayRate = Converter.ToDecimal(reader["Today"]),
                        YesterdayRate = Converter.ToDecimal(reader["Yesterday"])
                    });
                }

                reader.Close();
            }

            return balance;
        }
    }
}