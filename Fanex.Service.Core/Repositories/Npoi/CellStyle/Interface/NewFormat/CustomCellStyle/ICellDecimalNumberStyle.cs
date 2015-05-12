// -----------------------------------------------------------------------
// <copyright file="ICellDecimalNumberStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.NewFormat.CustomCellStyle
{
    using System.Drawing;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICellDecimalNumberStyle
    {
        XSSFCellStyle CreateDecimalNumberStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreatePositiveDecimalNumberStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateNegativeDecimalNumberStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateParenthesesPositiveDecimalNumberStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateParenthesesNegativeDecimalNumberStyle(Color backgroundColor, XSSFFont font);
    }
}