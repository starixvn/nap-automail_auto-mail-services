// -----------------------------------------------------------------------
// <copyright file="ICellTextStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.OldFormat.CustomCellStyle
{
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICellTextStyle
    {
        ICellStyle CreateLeftTextStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateRightTextStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateCenterTextStyle(short backgroundColorIndex, IFont font);
    }
}