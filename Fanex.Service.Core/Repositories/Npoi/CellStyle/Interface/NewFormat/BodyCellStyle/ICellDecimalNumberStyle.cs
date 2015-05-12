// -----------------------------------------------------------------------
// <copyright file="ICellDecimalNumberStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.NewFormat.BodyCellStyle
{
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICellDecimalNumberStyle
    {
        XSSFCellStyle DecimalNumberStyle { get; }

        XSSFCellStyle PositiveDecimalNumberStyle { get; }

        XSSFCellStyle NegativeDecimalNumberStyle { get; }

        XSSFCellStyle ParenthesesPositiveDecimalNumberStyle { get; }

        XSSFCellStyle ParenthesesNegativeDecimalNumberStyle { get; }
    }
}