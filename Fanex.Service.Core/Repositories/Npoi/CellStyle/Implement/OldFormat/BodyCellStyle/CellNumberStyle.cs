// -----------------------------------------------------------------------
// <copyright file="CellNumberStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Implement.OldFormat.BodyCellStyle
{
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.OldFormat.BodyCellStyle;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CellNumberStyle : ICellNumberStyle
    {
        private readonly HSSFWorkbook _workbook;
        private readonly IDataFormat _dataFormat;
        private readonly IFont _font;

        private ICellStyle _leftNumberStyle;
        private ICellStyle _rightNumberStyle;
        private ICellStyle _centerNumberStyle;        

        public CellNumberStyle(HSSFWorkbook workbook, IFont font)
        {
            this._workbook = workbook;
            this._font = font;
            _dataFormat = workbook.CreateDataFormat();
            CreateLeftNumberStyle();
            CreateRightNumberStyle();
            CreateCenterNumberStyle();
        }

        public ICellStyle LeftNumberStyle
        {
            get
            {
                return _leftNumberStyle;
            }
        }

        public ICellStyle RightNumberStyle
        {
            get
            {
                return _rightNumberStyle;
            }
        }

        public ICellStyle CenterNumberStyle
        {
            get
            {
                return _centerNumberStyle;
            }
        }

        private void CreateLeftNumberStyle()
        {
            this._leftNumberStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(this._leftNumberStyle, HSSFColor.White.Index);
            this._leftNumberStyle.Alignment = HorizontalAlignment.Left;
            this._leftNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NumberFormat);
            this._leftNumberStyle.SetFont(_font);
        }

        private void CreateRightNumberStyle()
        {
            this._rightNumberStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(this._rightNumberStyle, HSSFColor.White.Index);
            this._rightNumberStyle.Alignment = HorizontalAlignment.Right;
            this._rightNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NumberFormat);
            this._rightNumberStyle.SetFont(_font);
        }

        private void CreateCenterNumberStyle()
        {
            this._centerNumberStyle = this._workbook.CreateCellStyle();
            NpoiUtility.AssignOldFormatCellStyleData(this._centerNumberStyle, HSSFColor.White.Index);
            this._centerNumberStyle.Alignment = HorizontalAlignment.Center;
            this._centerNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NumberFormat);
            this._centerNumberStyle.SetFont(_font);
        }
    }
}