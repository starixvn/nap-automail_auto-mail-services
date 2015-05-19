using System;

namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Helper.Implement
{
    public static class NumberExtensions
    {
        /// <summary>
        /// Format int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Format(this int value)
        {
            return DoFormat(value, 0);
        }

        /// <summary>
        /// Format Object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Format(this object value)
        {
            return DoFormat(value, 0);
        }

        /// <summary>
        /// Format Decimal Value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decDigits"></param>
        /// <returns></returns>
        public static string Format(this decimal value, byte decDigits)
        {
            return DoFormat(value, decDigits);
        }

        /// <summary>
        /// Format Object value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decDigits"></param>
        /// <returns></returns>
        public static string Format(this object value, byte decDigits)
        {
            return DoFormat(value, decDigits);
        }

        /// <summary>
        /// Format Double Value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decDigits"></param>
        /// <returns></returns>
        public static string Format(this double value, byte decDigits)
        {
            return DoFormat(value, decDigits);
        }

        /// <summary>
        /// Format Float Value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decDigits"></param>
        /// <returns></returns>
        public static string Format(this float value, byte decDigits)
        {
            return DoFormat(value, decDigits);
        }

        /// <summary>
        /// Format specified number value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decDigits"></param>
        /// <returns>Value after rounding and formatting</returns>
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
    }
}
