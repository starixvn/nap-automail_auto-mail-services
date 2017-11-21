// -----------------------------------------------------------------------
// <copyright file="CellTextStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Implement.NewFormat.CustomCellStyle
{
    using System.Drawing;
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.NewFormat.CustomCellStyle;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CellTextStyle : ICellTextStyle
    {
        private readonly XSSFWorkbook _workbook;

        public CellTextStyle(XSSFWorkbook workbook)
        {
            this._workbook = workbook;
        }

        public XSSFCellStyle CreateLeftTextStyle(Color backgroundColor, XSSFFont font)
        {
            XSSFCellStyle leftTextStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(leftTextStyle, backgroundColor);
            leftTextStyle.Alignment = HorizontalAlignment.Left;
            leftTextStyle.SetFont(font);
            return leftTextStyle;
        }

        public XSSFCellStyle CreateRightTextStyle(Color backgroundColor, XSSFFont font)
        {
            XSSFCellStyle rightTextStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(rightTextStyle, backgroundColor);
            rightTextStyle.Alignment = HorizontalAlignment.Right;
            rightTextStyle.SetFont(font);
            return rightTextStyle;
        }

        public XSSFCellStyle CreateCenterTextStyle(Color backgroundColor, XSSFFont font)
        {
            XSSFCellStyle centerTextStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NpoiUtility.AssignNewFormatCellStyleData(centerTextStyle, backgroundColor);
            centerTextStyle.Alignment = HorizontalAlignment.Center;
            centerTextStyle.SetFont(font);
            return centerTextStyle;
        }
    }
}