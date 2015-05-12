// -----------------------------------------------------------------------
// <copyright file="ICellDecimalNumberStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.OldFormat.CustomCellStyle
{
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICellDecimalNumberStyle
    {
        ICellStyle CreateDecimalNumberStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreatePositiveDecimalNumberStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateNegativeDecimalNumberStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateParenthesesPositiveDecimalNumberStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateParenthesesNegativeDecimalNumberStyle(short backgroundColorIndex, IFont font);
    }
}