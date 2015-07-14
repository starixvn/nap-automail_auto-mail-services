namespace Fanex.DW.DailyBettypeWith3rdProducts.Business.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Daily BETTYPE With 3rd Products Criteria.
    /// </summary>
    public class DailyBettypeWith3rdProductsCriteria : CriteriaBase
    {
        public DailyBettypeWith3rdProductsCriteria()
        {
            var now = DateTime.Today;
            int year = now.Year;
            int month = now.Month;

            if (now.Day == 1)
            {
                if (month == 1)
                {
                    month = 12;
                    year = year - 1;
                }
                else
                {
                    month = month - 1;
                }

                this.DateStart = new DateTime(year, month , 1);                
            }
            else
            {
                this.DateStart = new DateTime(now.Year, now.Month, 1);                
            }

            this.DateEnd = now.AddDays(-1);

            if (AppSettings.IsManually)
            {
                this.DateStart = Convert.ToDateTime(AppSettings.DateStart);
                this.DateEnd = Convert.ToDateTime(AppSettings.DateEnd);
            }
        }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public int? PeriodId { get; set; }

        public int StatisticId { get; set; }

        public int? SportId { get; set; }

        public override string GetSettingKey()
        {
            return "DailyBettypeWith3rdProducts";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
