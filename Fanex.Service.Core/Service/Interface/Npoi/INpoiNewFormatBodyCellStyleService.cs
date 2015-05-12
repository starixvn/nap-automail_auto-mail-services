// -----------------------------------------------------------------------
// <copyright file="INpoiNewFormatBodyCellStyleService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Service.Interface.Npoi
{
    using Fanex.Service.Core.Model.NpoiNewFormat;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface INpoiNewFormatBodyCellStyleService
    {
        XSSFCellStyle LeftTextStyle { get; }

        XSSFCellStyle RightTextStyle { get; }

        XSSFCellStyle CenterTextStyle { get; }

        XSSFCellStyle LeftNumberStyle { get; }

        XSSFCellStyle RightNumberStyle { get; }

        XSSFCellStyle CenterNumberStyle { get; }

        XSSFCellStyle DecimalNumberStyle { get; }

        XSSFCellStyle PositiveDecimalNumberStyle { get; }

        XSSFCellStyle NegativeDecimalNumberStyle { get; }

        XSSFCellStyle ParenthesesPositiveDecimalNumberStyle { get; }

        XSSFCellStyle ParenthesesNegativeDecimalNumberStyle { get; }

        XSSFCellStyle PercentageStyle { get; }

        XSSFCellStyle PositivePercentageStyle { get; }

        XSSFCellStyle NegativePercentageStyle { get; }

        XSSFCellStyle ParenthesesPositivePercentageStyle { get; }

        XSSFCellStyle ParenthesesNegativePercentageStyle { get; }

        XSSFCellStyle CreateCellStyle(XSSFWorkbook workbook, BorderInformation border, AlignmentInformation alignment, XSSFFont font);
    }
}