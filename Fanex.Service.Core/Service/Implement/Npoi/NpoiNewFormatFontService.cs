// -----------------------------------------------------------------------
// <copyright file="NpoiNewFormatFontService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Service.Implement.Npoi
{
    using System.Drawing;
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Model.NpoiNewFormat;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NpoiNewFormatFontService : INpoiNewFormatFontService
    {
        private XSSFWorkbook _workbook;

        public NpoiNewFormatFontService(XSSFWorkbook workbook)
        {
            this._workbook = workbook;
        }

        public XSSFFont RegularFont
        {
            get
            {
                return CreateFont(_workbook, new FontInformation());
            }
        }

        public XSSFFont BlackBoldFont
        {
            get
            {
                return CreateBoldFont(_workbook, new FontInformation());
            }
        }

        public XSSFFont WhiteBoldFont
        {
            get
            {
                FontInformation fontInformation = new FontInformation(NPOIConstants.FontName, NPOIConstants.FontSize, Color.White);
                return CreateBoldFont(_workbook, fontInformation);
            }
        }

        public XSSFFont CreateFont(XSSFWorkbook workbook, FontInformation fontInformation)
        {
            XSSFFont font = workbook.CreateFont() as XSSFFont;
            font.FontName = fontInformation.FontName;
            font.FontHeightInPoints = fontInformation.FontSize;
            font.SetColor(new XSSFColor(fontInformation.FontColor));
            return font;
        }

        public XSSFFont CreateBoldFont(XSSFWorkbook workbook, FontInformation fontInformation)
        {
            XSSFFont font = CreateFont(workbook, fontInformation);
            font.Boldweight = (short)FontBoldWeight.Bold;
            return font;
        }
    }
}