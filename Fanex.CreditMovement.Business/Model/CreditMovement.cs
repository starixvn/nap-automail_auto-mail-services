// -----------------------------------------------------------------------
// <copyright file="CreditMovement.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CreditMovement.Business.Model
{
    /// <summary>
    /// Credit movement model.
    /// </summary>
    public class CreditMovement
    {
        public string Date { get; set; }

        public string Currency { get; set; }

        public string Site { get; set; }

        public string DownLinesInformation { get; set; }

        public decimal OriginalCurrency { get; set; }

        public decimal BaseCurrency { get; set; }
    }
}