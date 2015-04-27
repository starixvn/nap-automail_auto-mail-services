// -----------------------------------------------------------------------
// <copyright file="SignificantNumber.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.OandaExChangeRate.Business.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SignificantNumber
    {
        public static string GetSignificantFiguresAfterDecimalPoint(string input, int significantFiguresAfterDecimalPoint)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            string[] arrayStrings = input.Split('.');

            if (arrayStrings.Length == 1)
            {
                return input;
            }

            string stringBeforeDecimalPoint = arrayStrings[0];
            string stringAfterDecimalPoint = arrayStrings[1];

            stringAfterDecimalPoint = Rule1NineDecimalPlacesAfterDecimalPoint(stringAfterDecimalPoint);
            result = Rule2FourSignificantFiguresAfterDecimalPoint(significantFiguresAfterDecimalPoint, stringAfterDecimalPoint, stringBeforeDecimalPoint);
            result = Rule3SixSignificantFiguresInTotal(result);

            return result;
        }

        private static string Rule1NineDecimalPlacesAfterDecimalPoint(string stringAfterDecimalPoint)
        {
            if (stringAfterDecimalPoint.Length > 10)
            {
                stringAfterDecimalPoint = stringAfterDecimalPoint.Substring(0, 10);
            }

            return stringAfterDecimalPoint;
        }

        private static string Rule2FourSignificantFiguresAfterDecimalPoint(int significantFiguresAfterDecimalPoint, string stringAfterDecimalPoint, string stringBeforeDecimalPoint)
        {
            string result = string.Empty;

            List<string> listZeroes = new List<string>();
            List<string> listSignificant = new List<string>();
            bool isStillZero = true;
            bool isRoundUp = false;

            for (int i = 0; i < stringAfterDecimalPoint.Length; i++)
            {
                string digit = stringAfterDecimalPoint[i].ToString();
                if (digit == "0" && isStillZero)
                {
                    listZeroes.Add("0");
                }
                else
                {
                    isStillZero = false;

                    if (listSignificant.Count < significantFiguresAfterDecimalPoint)
                    {
                        listSignificant.Add(digit);
                    }
                    else
                    {
                        if (Convert.ToInt32(digit) >= 5)
                        {
                            isRoundUp = true;
                        }

                        break;
                    }
                }
            }

            stringAfterDecimalPoint = RoundStringAfterSignificant(stringAfterDecimalPoint, isRoundUp, listSignificant);

            result = stringBeforeDecimalPoint + "." + string.Join(string.Empty, listZeroes) + stringAfterDecimalPoint;

            return result;
        }

        private static string RoundStringAfterSignificant(string stringAfterDecimalPoint, bool isRoundUp, List<string> listSignificant)
        {
            if (isRoundUp && listSignificant.Count > 0)
            {
                stringAfterDecimalPoint = (Convert.ToInt32(string.Join(string.Empty, listSignificant)) + 1).ToString();
            }
            else
            {
                stringAfterDecimalPoint = string.Join(string.Empty, listSignificant);
            }

            return stringAfterDecimalPoint;
        }

        private static string Rule3SixSignificantFiguresInTotal(string input)
        {
            string result = input;

            if (string.IsNullOrEmpty(input))
            {
                return result;
            }

            string[] arrayStrings = input.Split('.');

            if (arrayStrings.Length == 1)
            {
                return result;
            }

            string stringBeforeDecimalPoint = arrayStrings[0];
            string stringAfterDecimalPoint = arrayStrings[1];

            int figuresInTotal = 6;
            int figuresBeforeDecimalPoint = Convert.ToDouble(stringBeforeDecimalPoint).ToString().Length;
            int figuresAfterDecimalPoint = figuresInTotal - figuresBeforeDecimalPoint;

            result = Rule2FourSignificantFiguresAfterDecimalPoint(figuresAfterDecimalPoint, stringAfterDecimalPoint, stringBeforeDecimalPoint);

            return result;
        }
    }
}
