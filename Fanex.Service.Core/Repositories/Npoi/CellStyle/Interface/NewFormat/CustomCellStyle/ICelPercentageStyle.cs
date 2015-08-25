// -----------------------------------------------------------------------
// <copyright file="ICelPercentageStyle.cs" company="Nexcel Solutions Vietnam.">
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
    public interface ICelPercentageStyle
    {
        XSSFCellStyle CreatePercentageStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreatePositivePercentageStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateNegativePercentageStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateParenthesesPositivePercentageStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateParenthesesNegativePercentageStyle(Color backgroundColor, XSSFFont font);
    }
}