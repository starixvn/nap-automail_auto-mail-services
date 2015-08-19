// -----------------------------------------------------------------------
// <copyright file="ProductsConstants.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business.Constants
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ProductsConstants
    {
        public static readonly List<string> ProductsHaveNoTOAndMargin = new List<string> 
        { 
            "RNG Casino", 
            "Racing",
            "Bingo",            
            "Bettrade",
        };

        public static readonly List<string> ProductsHaveNoNumberOfBetAndMargin = new List<string> 
        { 
            "AG Casino",
            "Macau",            
        };
    }
}
