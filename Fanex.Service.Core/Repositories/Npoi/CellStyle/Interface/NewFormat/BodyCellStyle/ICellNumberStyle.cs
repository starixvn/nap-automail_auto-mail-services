// -----------------------------------------------------------------------
// <copyright file="ICellNumberStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.NewFormat.BodyCellStyle
{
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICellNumberStyle
    {
        XSSFCellStyle LeftNumberStyle { get; }

        XSSFCellStyle RightNumberStyle { get; }

        XSSFCellStyle CenterNumberStyle { get; }
    }
}