// -----------------------------------------------------------------------
// <copyright file="ICelPercentageStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.OldFormat.CustomCellStyle
{
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICelPercentageStyle
    {
        ICellStyle CreatePercentageStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreatePositivePercentageStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateNegativePercentageStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateParenthesesPositivePercentageStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateParenthesesNegativePercentageStyle(short backgroundColorIndex, IFont font);
    }
}