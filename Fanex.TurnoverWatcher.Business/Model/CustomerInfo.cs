// -----------------------------------------------------------------------
// <copyright file="CustomerInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.TurnoverWatcher.Business.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerInfo
    {
        public int CustId { get; set; }

        public string CustomerName { get; set; }

        public string Currency { get; set; }

        public decimal MemberTurnover { get; set; }

        public decimal CompanyTurnover { get; set; }

        public decimal MaxTurnover { get; set; }

        public int PercentageUsed
        {
            get
            {
                if (0 == MaxTurnover)
                {
                    return 0;
                }

                decimal percentage = (CompanyTurnover / MaxTurnover) * 100;
                return (int)Math.Round(percentage, 0, MidpointRounding.AwayFromZero);                
            }
        }
    }
}