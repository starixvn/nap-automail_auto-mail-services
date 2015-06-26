// -----------------------------------------------------------------------
// <copyright file="NumberExtensions.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.GameWinLimitControl.Business.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class NumberExtensions
    {
        public static string CurrencyFormat(this decimal value)
        {
            return @String.Format("{0:0,0.00}", value);
        }

        /// <summary>
        /// Formats the integer.
        /// </summary>
        /// <param name="value">The value that needs to be formatted.</param>
        /// <returns>The formatted value.</returns>
        public static string FormatInteger(this int value)
        {
            return string.Format("{0:0,0}", value);
        }

        /// <summary>
        /// Formats the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value after format.</returns>
        public static string Format(this int value)
        {
            return DoFormat(value, 0);
        }

        /// <summary>
        /// Format Object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value after format.</returns>
        public static string Format(this object value)
        {
            return DoFormat(value, 0);
        }

        /// <summary>
        /// Format Decimal Value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decDigits">The digit.</param>
        /// <returns>The value after format.</returns>
        public static string Format(this decimal value, byte decDigits)
        {
            return DoFormat(value, decDigits);
        }

        /// <summary>
        /// Format Object value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decDigits">The digit.</param>
        /// <returns>The value after format.</returns>
        public static string Format(this object value, byte decDigits)
        {
            return DoFormat(value, decDigits);
        }

        /// <summary>
        /// Format Double Value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decDigits">The digit.</param>
        /// <returns>The value after format.</returns>
        public static string Format(this double value, byte decDigits)
        {
            return DoFormat(value, decDigits);
        }

        /// <summary>
        /// Format Float Value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decDigits">The digit.</param>
        /// <returns>The value after format.</returns>
        public static string Format(this float value, byte decDigits)
        {
            return DoFormat(value, decDigits);
        }

        /// <summary>
        /// Format specified number value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decDigits">The digit.</param>
        /// <returns>Value after rounding and formatting.</returns>
        private static string DoFormat(object value, byte decDigits)
        {
            if (value != null)
            {
                string formatString = "{0:n" + decDigits.ToString() + "}";
                string result = null;
                if (value != null)
                {
                    result = string.Format(formatString, value);
                }

                return result;
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public static string DecimalNumberFormat(decimal value)
        {
            return value.ToString("#,##0.00");
        }

        public static string DoubleNumberFormat(double value)
        {
            return value.ToString("#,##0.00");
        }

        public static string PercentFormat(double value)
        {
            return string.Format("{0:p2}", value);
        }
    }
}
