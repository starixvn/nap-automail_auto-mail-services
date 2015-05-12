// -----------------------------------------------------------------------
// <copyright file="CellNumberStyle.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Repositories.Npoi.CellStyle.Implement.NewFormat.CustomCellStyle
{
    using System.Drawing;
    using Fanex.Service.Core.Common;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Repositories.Npoi.CellStyle.Interface.NewFormat.CustomCellStyle;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CellNumberStyle : ICellNumberStyle
    {
        private XSSFWorkbook _workbook;
        private XSSFDataFormat _dataFormat;

        public CellNumberStyle(XSSFWorkbook workbook)
        {
            this._workbook = workbook;
            _dataFormat = workbook.CreateDataFormat() as XSSFDataFormat;
        }

        public XSSFCellStyle CreateLeftNumberStyle(Color backgroundColor, XSSFFont font)
        {
            XSSFCellStyle leftNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NPOIUtility.AssignNewFormatCellStyleData(leftNumberStyle, backgroundColor);
            leftNumberStyle.Alignment = HorizontalAlignment.Left;
            leftNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NumberFormat);
            leftNumberStyle.SetFont(font);
            return leftNumberStyle;
        }

        public XSSFCellStyle CreateRightNumberStyle(Color backgroundColor, XSSFFont font)
        {
            XSSFCellStyle rightNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NPOIUtility.AssignNewFormatCellStyleData(rightNumberStyle, backgroundColor);
            rightNumberStyle.Alignment = HorizontalAlignment.Right;
            rightNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NumberFormat);
            rightNumberStyle.SetFont(font);
            return rightNumberStyle;
        }

        public XSSFCellStyle CreateCenterNumberStyle(Color backgroundColor, XSSFFont font)
        {
            XSSFCellStyle centerNumberStyle = this._workbook.CreateCellStyle() as XSSFCellStyle;
            NPOIUtility.AssignNewFormatCellStyleData(centerNumberStyle, backgroundColor);
            centerNumberStyle.Alignment = HorizontalAlignment.Center;
            centerNumberStyle.DataFormat = _dataFormat.GetFormat(NPOIConstants.NumberFormat);
            centerNumberStyle.SetFont(font);
            return centerNumberStyle;
        }
    }
}