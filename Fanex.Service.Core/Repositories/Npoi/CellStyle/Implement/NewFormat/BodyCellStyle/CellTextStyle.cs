// -----------------------------------------------------------------------
// <copyright file="CellTextStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Implement.NewFormat.BodyCellStyle
{
    using System.Drawing;
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.NewFormat.BodyCellStyle;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CellTextStyle : ICellTextStyle
    {
        private readonly XSSFWorkbook _workbook;
        private readonly XSSFFont _font;

        private XSSFCellStyle _leftTextStyle;
        private XSSFCellStyle _rightTextStyle;
        private XSSFCellStyle _centerTextStyle;

        public CellTextStyle(XSSFWorkbook workbook, XSSFFont font)
        {
            this._workbook = workbook;
            this._font = font;
            CreateLeftTextStyle();
            CreateRightTextStyle();
            CreateCenterTextStyle();
        }

        public XSSFCellStyle LeftTextStyle
        {
            get
            {
                return _leftTextStyle;
            }
        }

        public XSSFCellStyle RightTextStyle
        {
            get
            {
                return _rightTextStyle;
            }
        }

        public XSSFCellStyle CenterTextStyle
        {
            get
            {
                return _centerTextStyle;
            }
        }

        private void CreateLeftTextStyle()
        {
            this._leftTextStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(this._leftTextStyle, Color.White);
            this._leftTextStyle.Alignment = HorizontalAlignment.Left;
            this._leftTextStyle.SetFont(_font);
        }

        private void CreateRightTextStyle()
        {
            this._rightTextStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(this._rightTextStyle, Color.White);
            this._rightTextStyle.Alignment = HorizontalAlignment.Right;
            this._rightTextStyle.SetFont(_font);
        }

        private void CreateCenterTextStyle()
        {
            this._centerTextStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(this._centerTextStyle, Color.White);
            this._centerTextStyle.Alignment = HorizontalAlignment.Center;
            this._centerTextStyle.SetFont(_font);
        }
    }
}