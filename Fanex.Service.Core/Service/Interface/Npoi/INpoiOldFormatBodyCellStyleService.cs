// -----------------------------------------------------------------------
// <copyright file="INpoiOldFormatCellStyleService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Service.Interface.Npoi
{
    using Fanex.Service.Core.Model.NpoiBase;
    using Fanex.Service.Core.Model.NpoiOldFormat;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface INpoiOldFormatBodyCellStyleService
    {
        ICellStyle LeftTextStyle { get; }

        ICellStyle RightTextStyle { get; }

        ICellStyle CenterTextStyle { get; }

        ICellStyle LeftNumberStyle { get; }

        ICellStyle RightNumberStyle { get; }

        ICellStyle CenterNumberStyle { get; }

        ICellStyle DecimalNumberStyle { get; }

        ICellStyle PositiveDecimalNumberStyle { get; }

        ICellStyle NegativeDecimalNumberStyle { get; }

        ICellStyle ParenthesesPositiveDecimalNumberStyle { get; }

        ICellStyle ParenthesesNegativeDecimalNumberStyle { get; }

        ICellStyle PercentageStyle { get; }

        ICellStyle PositivePercentageStyle { get; }

        ICellStyle NegativePercentageStyle { get; }

        ICellStyle ParenthesesPositivePercentageStyle { get; }

        ICellStyle ParenthesesNegativePercentageStyle { get; }

        ICellStyle CreateCellStyle(HSSFWorkbook workbook, BorderInformation border, AlignmentInformation alignment, IFont font);
    }
}