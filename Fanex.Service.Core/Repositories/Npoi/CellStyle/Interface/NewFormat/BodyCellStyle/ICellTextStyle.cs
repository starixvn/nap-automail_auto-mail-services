// -----------------------------------------------------------------------
// <copyright file="ICellTextStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.NewFormat.BodyCellStyle
{
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICellTextStyle
    {
        XSSFCellStyle LeftTextStyle { get; }

        XSSFCellStyle RightTextStyle { get; }

        XSSFCellStyle CenterTextStyle { get; }
    }
}