// -----------------------------------------------------------------------
// <copyright file="BaseService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Service
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using Fanex.Service.Core.Criteria;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class BaseService
    {
        public const string BetTrans14SPSuffix = "_14";
        public const string WarehouseSPSuffix = "_WH";

        public static Dictionary<int, string> ExchangeCurrencies
        {
            get
            {
                return GetExchangeCurrencies(new GetExchangeCurrenciesCriteria
                {
                    ExcludeTest = false
                });
            }
        }

        public static BalanceUp BalanceUp
        {
            get
            {
                return (new DynamicRepository()).Get<BalanceUp>(new BalanceUpCriteria());
            }
        }

        /// <summary>
        /// Gets the store mapping suffix.
        /// </summary>
        /// <param name="reportDate">The report date.</param>
        /// <returns>The store mapping suffix.</returns>
        public static string GetStoreMappingSuffix(DateTime reportDate)
        {
            if (reportDate > BalanceUp.MoveBetsDay)
            {
                return string.Empty;
            }
            else if (reportDate >= BalanceUp.BalanceUpDay)
            {
                return BetTrans14SPSuffix;
            }
            else
            {
                return WarehouseSPSuffix;
            }
        }

        /// <summary>
        /// Gets the exchange currencies.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The exchange currencies.</returns>
        public static Dictionary<int, string> GetExchangeCurrencies(GetExchangeCurrenciesCriteria criteria)
        {
            var exchangeCurrencies = new Dictionary<int, string>();
            try
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    IDataReader reader = objectDb.ExecuteReader(criteria.GetSpParams());
                    while (reader.Read())
                    {
                        int currencyId = Converter.ToInt(reader["exchangeid"]);
                        string currencyName = Converter.ToString2(reader["currency"]);
                        if (!exchangeCurrencies.ContainsKey(currencyId))
                        {
                            exchangeCurrencies.Add(currencyId, currencyName);
                        }
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return exchangeCurrencies;
        }
    }
}