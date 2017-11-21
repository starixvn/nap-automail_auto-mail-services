// -----------------------------------------------------------------------
// <copyright file="CellNumberStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Implement.OldFormat.CustomCellStyle
{
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.OldFormat.CustomCellStyle;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CellNumberStyle : ICellNumberStyle
    {
        private readonly HSSFWorkbook _workbook;
        private readonly IDataFormat _dataFormat;

        public CellNumberStyle(HSSFWorkbook workbook)
        {
            this._workbook = workbook;
            _dataFormat = workbook.CreateDataFormat();
        }

        public ICellStyle CreateLeftNumberStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle leftNumberStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(leftNumberStyle, backgroundColorIndex);
            leftNumberStyle.Alignment = HorizontalAlignment.Left;
            leftNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NumberFormat);
            leftNumberStyle.SetFont(font);
            return leftNumberStyle;
        }

        public ICellStyle CreateRightNumberStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle rightNumberStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(rightNumberStyle, backgroundColorIndex);
            rightNumberStyle.Alignment = HorizontalAlignment.Right;
            rightNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NumberFormat);
            rightNumberStyle.SetFont(font);
            return rightNumberStyle;
        }

        public ICellStyle CreateCenterNumberStyle(short backgroundColorIndex, IFont font)
        {
            ICellStyle centerNumberStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(centerNumberStyle, backgroundColorIndex);
            centerNumberStyle.Alignment = HorizontalAlignment.Center;
            centerNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NumberFormat);
            centerNumberStyle.SetFont(font);
            return centerNumberStyle;
        }
    }
}