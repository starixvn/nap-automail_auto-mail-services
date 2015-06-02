using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.NAPReport.BalanceMonthlyReport.Business.DataAccess;
using Fanex.NAPReport.BalanceMonthlyReport.Business.Model;
using Fanex.NAPReport.BalanceMonthlyReport.Business;

namespace Fanex.NAPReport.BalanceMonthlyReport.Business.Helper
{
    /// <summary>
    /// Summary description for AppCache
    /// </summary>
    public class CacheHelper
    {
        public static List<Exchange> Exchange
        {
            get
            {
                var helperDao = new HelperDAO();
                try
                {
                    return helperDao.GetExchange(new ExchangeCriteria());
                }
                catch (Exception ex)
                {
                    Helper.Logger.WriteLog(ex.Message);
                    
                }


                return new List<Exchange>();
            }
            
        }


       

        public static string GetExchangeName(int id)
        {
            string value = (from k in Exchange
                            where k.ExchangeID == id
                            select k.Currency).FirstOrDefault();
            return value;
        }
        public static string GetExchangeName(List<Exchange> exchange, int id)
        {
            string value = (from k in exchange
                            where k.ExchangeID == id
                            select k.Currency).FirstOrDefault();
            return value;
        }
    }
}
