// -----------------------------------------------------------------------
// <copyright file="SuperYesterdayBalanceRepository.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Repositories.SuperYesterdayBalance
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAPReport.ReportingServices.Business.Criteria.SuperYesterdayBalance;
    using Fanex.NAPReport.ReportingServices.Business.Model.SuperYesterdayBalance;
    using Fanex.Service.Core.Service;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SuperYesterdayBalanceRepository : ISuperYesterdayBalanceRepository
    {
        public List<SuperYesterdayBalanceDetail> GetSuperYesterdayBalanceDetail(GetSuperYesterdayBalanceCriteria criteria)
        {
            IDataReader reader;
            var superYesterdayBalanceDetails = new List<SuperYesterdayBalanceDetail>();
            var currencies = BaseService.ExchangeCurrencies;
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                reader = objectDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    var currencyId = Converter.ToInt(reader["currency"]);
                    superYesterdayBalanceDetails.Add(new SuperYesterdayBalanceDetail
                    {
                        Username = Converter.ToString2(reader["username"]),
                        Currency = currencies[currencyId],
                        Balance = Converter.ToDecimal(reader["Balance"]),
                        BaseBalance = Converter.ToDecimal(reader["BaseBalance"])
                    });
                }

                reader.Close();
            }

            return superYesterdayBalanceDetails;
        }
    }
}