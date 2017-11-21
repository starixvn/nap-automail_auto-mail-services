// -----------------------------------------------------------------------
// <copyright file="CellNumberStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Implement.NewFormat.BodyCellStyle
{
    using System.Drawing;
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.NewFormat.BodyCellStyle;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CellNumberStyle : ICellNumberStyle
    {
        private readonly XSSFWorkbook _workbook;
        private readonly XSSFDataFormat _dataFormat;
        private readonly XSSFFont _font;

        private XSSFCellStyle _leftNumberStyle;
        private XSSFCellStyle _rightNumberStyle;
        private XSSFCellStyle _centerNumberStyle;

        public CellNumberStyle(XSSFWorkbook workbook, XSSFFont font)
        {
            this._workbook = workbook;
            this._font = font;
            _dataFormat = workbook.CreateDataFormat() as XSSFDataFormat;
            CreateLeftNumberStyle();
            CreateRightNumberStyle();
            CreateCenterNumberStyle();
        }

        public XSSFCellStyle LeftNumberStyle
        {
            get
            {
                return _leftNumberStyle;
            }
        }

        public XSSFCellStyle RightNumberStyle
        {
            get
            {
                return _rightNumberStyle;
            }
        }

        public XSSFCellStyle CenterNumberStyle
        {
            get
            {
                return _centerNumberStyle;
            }
        }

        private void CreateLeftNumberStyle()
        {
            this._leftNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(this._leftNumberStyle, Color.White);
            this._leftNumberStyle.Alignment = HorizontalAlignment.Left;
            this._leftNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NumberFormat);
            this._leftNumberStyle.SetFont(_font);
        }

        private void CreateRightNumberStyle()
        {
            this._rightNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(this._rightNumberStyle, Color.White);
            this._rightNumberStyle.Alignment = HorizontalAlignment.Right;
            this._rightNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NumberFormat);
            this._rightNumberStyle.SetFont(_font);
        }

        private void CreateCenterNumberStyle()
        {
            this._centerNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(this._centerNumberStyle, Color.White);
            this._centerNumberStyle.Alignment = HorizontalAlignment.Center;
            this._centerNumberStyle.DataFormat = _dataFormat.GetFormat(NpoiConstants.NumberFormat);
            this._centerNumberStyle.SetFont(_font);
        }
    }
}