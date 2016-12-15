// -----------------------------------------------------------------------
// <copyright file="Product.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Product
    {
        public Product()
        {
            ProductName = string.Empty;
            Rows = new List<WinLoss>();
        }

        public string ProductName { get; set; }

        public List<WinLoss> Rows { get; set; }
    }
}
