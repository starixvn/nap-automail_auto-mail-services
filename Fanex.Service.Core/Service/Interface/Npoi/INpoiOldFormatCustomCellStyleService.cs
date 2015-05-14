// -----------------------------------------------------------------------
// <copyright file="INpoiOldFormatCustomCellStyleService.cs" company="">
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
    public interface INpoiOldFormatCustomCellStyleService
    {
        ICellStyle CreateLeftTextStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateRightTextStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateCenterTextStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateLeftNumberStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateRightNumberStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateCenterNumberStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateDecimalNumberStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreatePositiveDecimalNumberStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateNegativeDecimalNumberStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateParenthesesPositiveDecimalNumberStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateParenthesesNegativeDecimalNumberStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreatePercentageStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreatePositivePercentageStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateNegativePercentageStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateParenthesesPositivePercentageStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateParenthesesNegativePercentageStyle(short backgroundColorIndex, IFont font);

        ICellStyle CreateCellStyle(BorderInformation border, BackgroundInformation background, AlignmentInformation alignment, IFont font);
    }
}