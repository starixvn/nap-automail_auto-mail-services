// -----------------------------------------------------------------------
// <copyright file="CellTextStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Implement.OldFormat.BodyCellStyle
{
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.OldFormat.BodyCellStyle;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CellTextStyle : ICellTextStyle
    {
        private HSSFWorkbook _workbook;
        private ICellStyle _leftTextStyle;
        private ICellStyle _rightTextStyle;
        private ICellStyle _centerTextStyle;
        private IFont _font;

        public CellTextStyle(HSSFWorkbook workbook, IFont font)
        {
            this._workbook = workbook;
            this._font = font;
            CreateLeftTextStyle();
            CreateRightTextStyle();
            CreateCenterTextStyle();
        }

        public ICellStyle LeftTextStyle
        {
            get
            {
                return _leftTextStyle;
            }
        }

        public ICellStyle RightTextStyle
        {
            get
            {
                return _rightTextStyle;
            }
        }

        public ICellStyle CenterTextStyle
        {
            get
            {
                return _centerTextStyle;
            }
        }

        private void CreateLeftTextStyle()
        {
            this._leftTextStyle = this._workbook.CreateCellStyle();
            NPOIUtility.AssignOldFormatCellStyleData(this._leftTextStyle, HSSFColor.White.Index);
            this._leftTextStyle.Alignment = HorizontalAlignment.Left;
            this._leftTextStyle.SetFont(_font);
        }

        private void CreateRightTextStyle()
        {
            this._rightTextStyle = this._workbook.CreateCellStyle();
            NPOIUtility.AssignOldFormatCellStyleData(this._rightTextStyle, HSSFColor.White.Index);
            this._rightTextStyle.Alignment = HorizontalAlignment.Right;
            this._rightTextStyle.SetFont(_font);
        }

        private void CreateCenterTextStyle()
        {
            this._centerTextStyle = this._workbook.CreateCellStyle();
            NPOIUtility.AssignOldFormatCellStyleData(this._centerTextStyle, HSSFColor.White.Index);
            this._centerTextStyle.Alignment = HorizontalAlignment.Center;
            this._centerTextStyle.SetFont(_font);
        }
    }
}