namespace Fanex.OandaExChangeRate.Business.DataAccess
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.OandaExChangeRate.Business.Model;

    public class ExchangeRateDAO : BaseDataAccess, IExchangeRateDAO
    {
        public List<string> GetCurrencyCodeList()
        {
            var currencyCodeList = new List<string>();            
            using (ObjectDb db = new ObjectDb("Acc_ExchangeCodeSel"))
            {
                var reader = db.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var code = reader["Code"] != DBNull.Value
                                       ? reader.GetString(reader.GetOrdinal("Code"))
                                       : string.Empty;
                        currencyCodeList.Add(code.Trim());
                    }
                }
            }

            return currencyCodeList;
        }

        /// <summary>
        /// Update rate from one currency to base currencies
        /// </summary>
        /// <param name="fromCurrency"></param>
        /// <param name="runDate"></param>
        /// <param name="toCurrency">Base currencies list with rate ex: {USD, 0.02}</param>
        /// <returns></returns>
        public Result UpdateRate(string fromCurrency, DateTime runDate, Dictionary<string, double> toCurrency)
        {
            var result = new Result { ErrorCode = 0, ErrorMsg = string.Empty };

            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("rundate", runDate);
                parameters.Add("code", fromCurrency);
                parameters.Add("errcode", 0);
                parameters.Add("errmsg", string.Empty);

                foreach (var currency in toCurrency)
                {
                    parameters.Add(currency.Key.ToLower(), currency.Value);
                }

                using (ObjectDb db = new ObjectDb("Acc_ExchangeDailyIns"))
                {
                    db.AssignParamValues(parameters);
                    db.ExecuteNonQuery();

                    var outputParameters = db.GetOuputParams();
                    
                    result.ErrorCode = Converter.ToInt(outputParameters["errcode"]);
                    result.ErrorMsg = Convert.ToString(outputParameters["errmsg"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public Result UpdateIbcRate()
        {
            var result = new Result { ErrorCode = 0, ErrorMsg = string.Empty };
            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("errcode", 0);
                parameters.Add("errmsg", string.Empty);

                using (ObjectDb db = new ObjectDb("Acc_ExchangeDaily_ActualRateUpd"))
                {
                    db.AssignParamValues(parameters);
                    db.ExecuteNonQuery();

                    var outputParameters = db.GetOuputParams();

                    result.ErrorCode = Converter.ToInt(outputParameters["errcode"]);
                    result.ErrorMsg = Convert.ToString(outputParameters["errmsg"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
