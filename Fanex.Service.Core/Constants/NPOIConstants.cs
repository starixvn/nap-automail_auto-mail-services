// -----------------------------------------------------------------------
// <copyright file="NPOIConstants.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Constants
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class NPOIConstants
    {
        public const int FontWidth = 30;
        public const int FontHeight = 20;
        public const int FontSize = 8;
        public const int MaxRowPerSheet = 20000;
        public const int RowHeight = 300;

        public const string FontName = "Tahoma";
        public const string NumberFormat = "#,##0";
        public const string PositiveNumberFormat = "[>0][Red]#,##0;[<0]-#,##0;#,##0";
        public const string NegativeNumberFormat = "[>0]#,##0;[<0][Red]-#,##0;#,##0";
        public const string NegativeDecimalNumberFormat = "[>0]#,##0.00;[<0][Red]-#,##0.00;#,##0.00";
        public const string PositiveDecimalNumberFormat = "[>0][Red]#,##0.00;[<0]-#,##0.00;#,##0.00";
        public const string ParenthesesNegativeDecimalNumberFormat = "[>0]#,##0.00;[<0][Red](-#,##0.00);#,##0.00";
        public const string ParenthesesPositiveDecimalNumberFormat = "[>0][Red](#,##0.00);[<0]-#,##0.00;#,##0.00";
        public const string NormalDecimalNumberFormat = "#,##0.00;-#,##0.00";
        public const string NegativePercentFormat = "[>0]#,##0.00%;[<0][Red]-#,##0.00%;#,##0.00%";
        public const string PositivePercentFormat = "[>0][Red]#,##0.00%;[<0]-#,##0.00%;#,##0.00%";
        public const string ParenthesesNegativePercentFormat = "[>0]#,##0.00%;[<0][Red](-#,##0.00%);#,##0.00%";
        public const string ParenthesesPositivePercentFormat = "[>0][Red](#,##0.00%);[<0]-#,##0.00%;#,##0.00%";
        public const string ParenthesesBlackNegativeDecimalNumberFormat = "[>0]#,##0.00;[<0](-#,##0.00);#,##0.00";
        public const string ParenthesesBlackPositiveDecimalNumberFormat = "[>0](#,##0.00);[<0]-#,##0.00;#,##0.00";
        public const string ParenthesesUnsignedNegativeDecimalNumberFormat = "[>0]#,##0.00;[<0](#,##0.00);#,##0.00";

        public const string NormalPercentFormat = "#,##0.00%;-#,##0.00%";

        public const int VerticalAlignTop = 0;
        public const int VerticalAlignCenter = 1;
        public const int VerticalAlignBottom = 2;
        public const int VerticalAlignJustify = 3;
        public const int VerticalAlignDistributed = 4;

        public const int HeaderColorIndex = 54;
        public const int EvenRowColorIndex = 55;
        public const int OddRowColorIndex = 56;
        public const int SubTotalColorIndex = 57;
        public const int GrandTotalColorIndex = 58;
        public const int GrandTotalBorderColorIndex = 59;
        public const int SpecialRowColorIndex = 60;
    }
}
