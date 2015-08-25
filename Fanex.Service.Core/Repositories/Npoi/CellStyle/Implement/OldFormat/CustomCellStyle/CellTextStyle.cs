// -----------------------------------------------------------------------
// <copyright file="CellTextStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Implement.OldFormat.CustomCellStyle
{
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.OldFormat.CustomCellStyle;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CellTextStyle : ICellTextStyle
    {
        private HSSFWorkbook _workbook;

        public CellTextStyle(HSSFWorkbook workbook)
        {
            this._workbook = workbook;
        }

        public ICellStyle CreateLeftTextStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle leftTextStyle = this._workbook.CreateCellStyle();
            NPOIUtility.AssignOldFormatCellStyleData(leftTextStyle, backgroundColorIndex);
            leftTextStyle.Alignment = HorizontalAlignment.Left;
            leftTextStyle.SetFont(font);
            return leftTextStyle;
        }

        public ICellStyle CreateRightTextStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle rightTextStyle = this._workbook.CreateCellStyle();
            NPOIUtility.AssignOldFormatCellStyleData(rightTextStyle, backgroundColorIndex);
            rightTextStyle.Alignment = HorizontalAlignment.Right;
            rightTextStyle.SetFont(font);
            return rightTextStyle;
        }

        public ICellStyle CreateCenterTextStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle centerTextStyle = this._workbook.CreateCellStyle();
            NPOIUtility.AssignOldFormatCellStyleData(centerTextStyle, backgroundColorIndex);
            centerTextStyle.Alignment = HorizontalAlignment.Center;
            centerTextStyle.SetFont(font);
            return centerTextStyle;
        }
    }
}