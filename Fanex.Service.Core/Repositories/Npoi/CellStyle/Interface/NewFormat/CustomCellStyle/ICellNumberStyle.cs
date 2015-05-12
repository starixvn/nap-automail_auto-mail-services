// -----------------------------------------------------------------------
// <copyright file="ICellNumberStyle.cs" company="Nexcel Solutions Vietnam.">
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
    public interface ICellNumberStyle
    {
        XSSFCellStyle CreateLeftNumberStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateRightNumberStyle(Color backgroundColor, XSSFFont font);

        XSSFCellStyle CreateCenterNumberStyle(Color backgroundColor, XSSFFont font);
    }
}