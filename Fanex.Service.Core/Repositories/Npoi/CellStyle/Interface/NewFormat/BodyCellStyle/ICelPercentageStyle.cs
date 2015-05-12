// -----------------------------------------------------------------------
// <copyright file="ICelPercentageStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.NewFormat.BodyCellStyle
{
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICelPercentageStyle
    {
        XSSFCellStyle PercentageStyle { get; }

        XSSFCellStyle PositivePercentageStyle { get; }

        XSSFCellStyle NegativePercentageStyle { get; }

        XSSFCellStyle ParenthesesPositivePercentageStyle { get; }

        XSSFCellStyle ParenthesesNegativePercentageStyle { get; }
    }
}