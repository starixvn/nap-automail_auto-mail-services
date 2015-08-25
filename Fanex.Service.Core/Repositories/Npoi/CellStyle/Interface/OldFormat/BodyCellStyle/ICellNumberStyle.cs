// -----------------------------------------------------------------------
// <copyright file="ICellNumberStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.OldFormat.BodyCellStyle
{
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICellNumberStyle
    {
        ICellStyle LeftNumberStyle { get; }

        ICellStyle RightNumberStyle { get; }

        ICellStyle CenterNumberStyle { get; }
    }
}