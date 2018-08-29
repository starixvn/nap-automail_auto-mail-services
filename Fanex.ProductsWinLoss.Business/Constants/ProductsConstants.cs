// -----------------------------------------------------------------------
// <copyright file="ProductsConstants.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business.Constants
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ProductsConstants
    {
        public static List<string> ProductsHaveNoTOAndMargin
        {
            get
            {
                return new List<string> { "RNG Casino", "Racing", "Bingo", "Bettrade" };
            }
        }

        public static List<string> ProductsHaveNoNumberOfBetAndMargin
        {
            get
            {
                return new List<string> { "AG Casino", "Macau" };
            }
        }

        public static List<string> IgnoreProductsWhenCalcTotal
        {
            get
            {
                return new List<string>
                {
                    "casino (amount due to jackpot pool)",
                    "poker games (due to partners)"
                };
            }
        }
    }
}