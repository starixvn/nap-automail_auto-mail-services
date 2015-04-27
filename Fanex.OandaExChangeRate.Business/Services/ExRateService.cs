namespace Fanex.OandaExChangeRate.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.UI;
    using Fanex.OandaExChangeRate.Business.Common;
    using Fanex.OandaExChangeRate.Business.DataAccess;
    using Fanex.OandaExChangeRate.Business.Infrastructure.Helper;
    using Fanex.OandaExChangeRate.Business.Infrastructure.IoC;
    using Fanex.OandaExChangeRate.Business.Model;
    using Fanex.OandaExChangeRate.Business.Services.CurrencyConverter;
    using Fanex.OandaExChangeRate.Business.Utility;

    public class ExRateService
    {
        private IExchangeRateDAO _exchagenRateDAO;

        public ExRateService()
        {
           _exchagenRateDAO = CommonBootStrapper.Locator.GetInstance<IExchangeRateDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromCurrency">Ex: USD</param>
        /// <param name="toCurrency">Ex: VND</param>
        /// <param name="rateDate">Rate Date: String Short Datetime</param>
        /// <returns></returns>
        public double GetExchangeRate(string fromCurrency, string toCurrency, string rateDate)
        {
            var currencyConverter = BaseCurrencyConvertor.Instance;
            double rate = 0;
            string rate_API = "0";
            try
            {
                if (AppSettings.IsSimulateRate)
                {
                    rate = currencyConverter.GetExchageRate_Simulate(fromCurrency, toCurrency, rateDate);
                }
                else
                {
                    rate_API = currencyConverter.GetExchageRate(fromCurrency, toCurrency, rateDate);
                }

                var result = SignificantNumber.GetSignificantFiguresAfterDecimalPoint(rate_API, 4);
                rate = Convert.ToDouble(result);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return rate;
        }

        public Result UpdateRate(string[] listBaseCurrency)
        {
            var updateResult = new Result { ErrorCode = 0, ErrorMsg = string.Empty };
            SQLiteDataAccess _log = new SQLiteDataAccess();

            var listCurrency = _exchagenRateDAO.GetCurrencyCodeList();
            if (listCurrency.Count == 0 || listBaseCurrency.Length == 0)
            {
                return updateResult;
            }

            var listError = new List<Result>();
            var toDayRate = string.Empty;
            var stringWriter = new StringWriter();
            var htmlContent = new HtmlTextWriter(stringWriter);

            htmlContent.AddAttribute(HtmlTextWriterAttribute.Border, "1");
            htmlContent.RenderBeginTag(HtmlTextWriterTag.Table);
            ////header
            htmlContent.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "#AD848E");
            htmlContent.AddAttribute(HtmlTextWriterAttribute.Align, "center");
            htmlContent.RenderBeginTag(HtmlTextWriterTag.Tr);
            htmlContent.RenderBeginTag(HtmlTextWriterTag.Td);
            htmlContent.Write("Currency");
            htmlContent.RenderEndTag();

            foreach (var baseCurrency in listBaseCurrency)
            {
                htmlContent.RenderBeginTag(HtmlTextWriterTag.Td);
                htmlContent.Write(baseCurrency);
                htmlContent.RenderEndTag();
            }

            htmlContent.RenderEndTag();
            var exchangeRateList = new Dictionary<string, double>();
            foreach (var currency in listCurrency)
            {
                if (currency.ToLower().Equals("c") || currency.ToLower().Equals("h") || currency.ToLower().Equals("uus"))
                {
                    continue;
                }

                exchangeRateList = new Dictionary<string, double>();
                htmlContent.RenderBeginTag(HtmlTextWriterTag.Tr);
                htmlContent.RenderBeginTag(HtmlTextWriterTag.Td);
                htmlContent.Write(currency);
                htmlContent.RenderEndTag();
                foreach (var baseCurrency in listBaseCurrency)
                {
                    htmlContent.RenderBeginTag(HtmlTextWriterTag.Td);
                    double rate = 0;                   
                    rate = GetExchangeRate(currency, baseCurrency, DateTime.Now.ToShortDateString());

                    if (rate == 0)
                    {
                        throw new Exception("currency: " + currency + ", baseCurrency: " + baseCurrency + ", rate: " + rate.ToString());
                    }
               
                    exchangeRateList.Add(baseCurrency, rate);
                    htmlContent.Write(rate);
                    htmlContent.RenderEndTag();                    
                }

                var result = new Result();
                if (!AppSettings.IsInsertHistoryRate)
                {
                    result = _exchagenRateDAO.UpdateRate(currency, DateTime.Now, exchangeRateList);
                    result = _exchagenRateDAO.UpdateRate(currency, DateTime.Now.AddDays(1), exchangeRateList);
                }

                if (0 != result.ErrorCode)
                {
                    listError.Add(result);
                }

                htmlContent.RenderEndTag();

                string currentCurrency = "Currency: " + currency + " - DateTime: " + DateTime.Now.ToString();
                _log.UpdateCurrentCurrency(AppSettings.Id, currentCurrency);
            }

            htmlContent.RenderEndTag();

            if (!AppSettings.IsInsertHistoryRate)
            {
                _exchagenRateDAO.UpdateRate("UUS", DateTime.Now, exchangeRateList);
                _exchagenRateDAO.UpdateRate("UUS", DateTime.Now.AddDays(1), exchangeRateList);
            }
            
            string content = string.Empty;
            if (listError.Count > 0)
            {
                foreach (var result in listError)
                {
                    content += result.ErrorCode + "<br>";
                    content += result.ErrorMsg + "<br><br>";
                }

                Mail.SendMail(content);
            }
            else
            {
                ////Send mail today rate
                if (!AppSettings.IsInsertHistoryRate)
                {
                    Mail.SendMail("Today Rate: " + stringWriter.ToString());
                }
            }

            return updateResult;
        }

        public Result UpdateRate(string[] listBaseCurrency, DateTime exchangeDate)
        {
            var updateResult = new Result { ErrorCode = 0, ErrorMsg = string.Empty };

            var listCurrency = _exchagenRateDAO.GetCurrencyCodeList();
            if (listCurrency.Count == 0 || listBaseCurrency.Length == 0)
            {
                return updateResult;
            }

            var listError = new List<Result>();
            var toDayRate = string.Empty;
            var stringWriter = new StringWriter();
            var htmlContent = new HtmlTextWriter(stringWriter);

            htmlContent.AddAttribute(HtmlTextWriterAttribute.Border, "1");
            htmlContent.RenderBeginTag(HtmlTextWriterTag.Table);
            ////header
            htmlContent.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "#AD848E");
            htmlContent.AddAttribute(HtmlTextWriterAttribute.Align, "center");
            htmlContent.RenderBeginTag(HtmlTextWriterTag.Tr);
            htmlContent.RenderBeginTag(HtmlTextWriterTag.Td);
            htmlContent.Write("Currency");
            htmlContent.RenderEndTag();
            foreach (var baseCurrency in listBaseCurrency)
            {
                htmlContent.RenderBeginTag(HtmlTextWriterTag.Td);
                htmlContent.Write(baseCurrency);
                htmlContent.RenderEndTag();
            }

            htmlContent.RenderEndTag();
            var exchangeRateList = new Dictionary<string, double>();

            DateTime dateStoreInDB = AppSettings.IsTest ? exchangeDate.AddDays(1) : exchangeDate.AddDays(0);

            foreach (var currency in listCurrency)
            {
                exchangeRateList = new Dictionary<string, double>();
                htmlContent.RenderBeginTag(HtmlTextWriterTag.Tr);
                htmlContent.RenderBeginTag(HtmlTextWriterTag.Td);
                htmlContent.Write(currency);
                htmlContent.RenderEndTag();
                foreach (var baseCurrency in listBaseCurrency)
                {
                    htmlContent.RenderBeginTag(HtmlTextWriterTag.Td);
                    var rate = GetExchangeRate(currency, baseCurrency, exchangeDate.ToShortDateString());
                    exchangeRateList.Add(baseCurrency, rate);
                    htmlContent.Write(rate);
                    htmlContent.RenderEndTag();
                }

                var result1 = _exchagenRateDAO.UpdateRate(currency, dateStoreInDB, exchangeRateList);

                if (!AppSettings.IsInsertHistoryRate)
                {
                    result1 = _exchagenRateDAO.UpdateRate(currency, dateStoreInDB, exchangeRateList);
                    result1 = _exchagenRateDAO.UpdateRate(currency, dateStoreInDB.AddDays(1), exchangeRateList);
                }

                if (0 != result1.ErrorCode)
                {
                    listError.Add(result1);
                }

                htmlContent.RenderEndTag();
            }

            htmlContent.RenderEndTag();
            _exchagenRateDAO.UpdateRate("UUS", dateStoreInDB, exchangeRateList);

            if (!AppSettings.IsInsertHistoryRate)
            {
                _exchagenRateDAO.UpdateRate("UUS", dateStoreInDB, exchangeRateList);
                _exchagenRateDAO.UpdateRate("UUS", dateStoreInDB.AddDays(1), exchangeRateList);
            }

            var content = string.Empty;
            if (listError.Count > 0)
            {
                foreach (var result in listError)
                {
                    content += result.ErrorCode + "<br>";
                    content += result.ErrorMsg + "<br><br>";
                }

                Mail.SendMail(content);
            }
            else
            {
                ////Send mail today rate
                if (!AppSettings.IsInsertHistoryRate)
                {
                    Mail.SendMail("Today Rate:\n" + stringWriter.ToString());
                }
            }

            return updateResult;
        }

        public Result UpdateIbcRate()
        {
            var result = _exchagenRateDAO.UpdateIbcRate();
            if (result.ErrorCode != 0)
            {
                throw new Exception(result.ErrorMsg);
            }

            return result;
        }
    }
}
