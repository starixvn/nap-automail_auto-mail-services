// -----------------------------------------------------------------------
// <copyright file="ICellDecimalNumberStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.OldFormat.BodyCellStyle
{
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICellDecimalNumberStyle
    {
        ICellStyle DecimalNumberStyle { get; }

        ICellStyle PositiveDecimalNumberStyle { get; }

        ICellStyle NegativeDecimalNumberStyle { get; }

        ICellStyle ParenthesesPositiveDecimalNumberStyle { get; }

        ICellStyle ParenthesesNegativeDecimalNumberStyle { get; }
    }
}