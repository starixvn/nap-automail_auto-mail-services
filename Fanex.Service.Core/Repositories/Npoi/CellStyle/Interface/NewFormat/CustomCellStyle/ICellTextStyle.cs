// -----------------------------------------------------------------------
// <copyright file="ICellTextStyle.cs" company="Nexcel Solutions Vietnam.">
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
    public interface ICellTextStyle
    {
        XSSFCellStyle CreateLeftTextStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateRightTextStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateCenterTextStyle(Color backgroundColor, XSSFFont font);
    }
}