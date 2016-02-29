namespace Fanex.NAPReport.HorseTransaction3rd.Business.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using DNAide.Utils;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Criteria;

    public class ComparisionManager
    {
        private HorseRacing3rdService _service = new HorseRacing3rdService();
        private IExchangeService _exchangeService = new ExchangeService();

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2100:CodeLineMustNotBeLongerThan",
           Justification = "Reviewed. Suppression is OK here.")]
        public string GetComparision(DateTime winlostdate, int racingId)
        {            
            var winLossCriteria = new GetRacingWinLossCriteria() { ReportType = Convert.ToInt32(racingId), WinLostDate = winlostdate };
            var winlostHR_OpenTote = _service.WinlostHR_OpenTote(winLossCriteria);

            decimal ibcTO = 0;
            decimal ibcDividendWon = 0;
            int currentPoolCurrency = 0, lastPoolCurency = 0;
            var ibcPoolCurrency = string.Empty;

            if (winlostHR_OpenTote.Count > 0)
            {
                foreach (var item in winlostHR_OpenTote)
                {
                    currentPoolCurrency = item.Currency;
                    if (currentPoolCurrency != lastPoolCurency && ibcPoolCurrency.Contains(currentPoolCurrency.ToString()) == false)
                    {
                        ibcPoolCurrency += currentPoolCurrency + ";";
                    }

                    lastPoolCurency = currentPoolCurrency;
                }
            }           
            
            var criteria = new GetRacingTo3rdCriteria() { WinLostDate = winlostdate, ReportType = Convert.ToInt32(racingId) };
            var openToteTO3rd = _service.Horse_OpenToteTO3rd(criteria);
            decimal _3rdTO = 0, _3rdDividendWon = 0;
            string color_TO = string.Empty, color_dividend = string.Empty;                        
            
            var html = new StringBuilder();
            html.Append("<table style='background-color: #999999; font-family:Tahoma,Arial,helvetica,sans-serif;font-size:8pt'>");
            html.Append("<thead>");
            html.Append("<tr style='height:20' bgcolor='#d0e8fb'>");
            html.Append("<th rowspan='2' width='60' align='center' valign='center'><b>Currency</b></th>");
            html.Append("<th colspan='2' width='200' align='center' valign='center'><b>Partner</b></th>");
            html.Append("<th colspan='2' width='200' align='center' valign='center'><b>Company</b></th>");
            html.Append("</tr>");

            html.Append("<tr style='height:20' bgcolor='#d0e8fb'>");
            html.Append("<th width='100' align='center' valign='center'><b>TO</b></th>");
            html.Append("<th width='100' align='center' valign='center'><b>Dividend WON</b></th>");
            html.Append("<th width='100' align='center' valign='center'><b>TO</b></th>");
            html.Append("<th width='100' align='center' valign='center'><b>Dividend WON</b></th>");
            html.Append("</tr>");

            foreach (string poolCurrencyComparation in ibcPoolCurrency.Split(';'))
            {
                if (!string.IsNullOrEmpty(poolCurrencyComparation))
                {
                    var getExchangeCriteria = new GetExchangeCriteria();
                    var exchangeList = _exchangeService.GetExchange(getExchangeCriteria);
                    var currencyName = exchangeList.Where(i => i.ExchangeID == Convert.ToInt32(poolCurrencyComparation)).FirstOrDefault().Currency;
                    currencyName = currencyName.Equals("EURO") ? "EUR" : currencyName;
                    
                    var ibcCondition = "PoolCurrency = '" + poolCurrencyComparation + "'";
                    var _3rdCondition = "currencyName = '" + currencyName + "'";

                    ibcDividendWon = winlostHR_OpenTote.Where(i => i.Currency == Convert.ToInt32(poolCurrencyComparation)).Sum(i => i.DividendWon_PoolCurr);
                    ibcTO = winlostHR_OpenTote.Where(i => i.Currency == Convert.ToInt32(poolCurrencyComparation)).Sum(i => i.TotalTO_Partner_PoolCurr);
                    _3rdTO = openToteTO3rd.Where(i => i.Currency == _3rdCondition).Sum(i => i.To3rd);
                    _3rdDividendWon = openToteTO3rd.Where(i => i.Currency == _3rdCondition).Sum(i => i.DividendWon3rd);

                    if (IsMatched(_3rdTO, ibcTO))
                    {
                        color_TO = "blue";
                    }
                    else
                    {
                        color_TO = "red";
                    }

                    if (IsMatched(_3rdDividendWon, ibcDividendWon))
                    {
                        color_dividend = "blue";
                    }
                    else
                    {
                        color_dividend = "red";
                    }

                    html.Append("<tr style='height:20' bgcolor='white'>");
                    html.Append("<td style='text-align:center' nowrap>" + currencyName + "</td>");
                    html.Append("<td style='text-align:right; color:" + color_TO + "' nowrap>" + (_3rdTO == 0 && _3rdDividendWon == 0 ? "N/A" : NumberFormat.DecFormat(_3rdTO, 2)) + "</td>");
                    html.Append("<td style='text-align:right; color:" + color_dividend + "' nowrap>" + (_3rdTO == 0 && _3rdDividendWon == 0 ? "N/A" : NumberFormat.DecFormat(_3rdDividendWon, 2)) + "</td>");
                    html.Append("<td style='text-align:right; color:" + color_TO + "' nowrap>" + NumberFormat.DecFormat(ibcTO, 2) + "</td>");
                    html.Append("<td style='text-align:right; color:" + color_dividend + "' nowrap>" + NumberFormat.DecFormat(ibcDividendWon, 2) + "</td>");
                    html.Append("</tr>");
                }
            }

            html.Append("</table>");

            return html.ToString();
        }

        private bool IsMatched(decimal num1, decimal num2)
        {
            var result = Math.Abs(num1 - num2);
            return result <= 1;
        }
    }
}
