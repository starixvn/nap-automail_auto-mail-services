// -----------------------------------------------------------------------
// <copyright file="INpoiNewFormatCustomCellStyleService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Service.Interface.Npoi
{
    using System.Drawing;
    using Fanex.Service.Core.Model.NpoiNewFormat;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface INpoiNewFormatCustomCellStyleService
    {
        XSSFCellStyle CreateCellStyle(XSSFWorkbook workbook, BorderInformation border, BackgroundInformation background, AlignmentInformation alignment, XSSFFont font);

        XSSFCellStyle CreateLeftTextStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateRightTextStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateCenterTextStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateLeftNumberStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateRightNumberStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateCenterNumberStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateDecimalNumberStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreatePositiveDecimalNumberStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateNegativeDecimalNumberStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateParenthesesPositiveDecimalNumberStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateParenthesesNegativeDecimalNumberStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreatePercentageStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreatePositivePercentageStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateNegativePercentageStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateParenthesesPositivePercentageStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateParenthesesNegativePercentageStyle(Color backgroundColor, XSSFFont font);
    }
}