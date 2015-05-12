// -----------------------------------------------------------------------
// <copyright file="ICelPercentageStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.OldFormat.BodyCellStyle
{
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICelPercentageStyle
    {
        ICellStyle PercentageStyle { get; }

        ICellStyle PositivePercentageStyle { get; }

        ICellStyle NegativePercentageStyle { get; }

        ICellStyle ParenthesesPositivePercentageStyle { get; }

        ICellStyle ParenthesesNegativePercentageStyle { get; }
    }
}