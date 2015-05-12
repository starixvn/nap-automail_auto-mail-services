// -----------------------------------------------------------------------
// <copyright file="ICellNumberStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.OldFormat.CustomCellStyle
{
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICellNumberStyle
    {
        ICellStyle CreateLeftNumberStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateRightNumberStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateCenterNumberStyle(short backgroundColorIndex, IFont font);
    }
}