namespace Fanex.NAPReport.RngCasino.Business.Helper.Implement
{
    using System;

    public static class NumberExtension
    {
        /// <summary>
        /// Format Number.
        /// </summary>
        /// <param name="value">Value of win loss.</param>
        /// <param name="decDigits">Number of digit of win loss value.</param>
        /// <returns>Format of win loss value.</returns>
        public static string WinLossFormat(object value, byte decDigits)
        {
            if (value != null)
            {
                if (decDigits == 0)
                {
                    return @String.Format("{0:0,0}", value);
                }

                return @String.Format("{0:0,0.00}", value);
            }
            else
            {
                throw new NullReferenceException();
            }
        }
    }
}
