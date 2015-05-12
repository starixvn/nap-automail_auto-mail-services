// -----------------------------------------------------------------------
// <copyright file="NpoiOldFormatFontService.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Service.Implement.Npoi
{
    using Fanex.Service.Core.Constants;
    using Fanex.Service.Core.Model.NpoiOldFormat;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NpoiOldFormatFontService : INpoiOldFormatFontService
    {
        private HSSFWorkbook _workbook;

        public NpoiOldFormatFontService(HSSFWorkbook workbook)
        {
            this._workbook = workbook;
        }

        public IFont RegularFont
        {
            get
            {
                return CreateFont(_workbook, new FontInformation());
            }
        }

        public IFont BlackBoldFont
        {
            get
            {
                return CreateBoldFont(_workbook, new FontInformation());
            }
        }

        public IFont WhiteBoldFont
        {
            get
            {
                FontInformation fontInformation = new FontInformation(NPOIConstants.FontName, NPOIConstants.FontSize, HSSFColor.White.Index);
                return CreateBoldFont(_workbook, fontInformation);
            }
        }

        public IFont CreateFont(HSSFWorkbook workbook, FontInformation fontInformation)
        {
            IFont font = workbook.CreateFont();
            font.FontName = fontInformation.FontName;
            font.FontHeightInPoints = fontInformation.FontSize;
            font.Color = fontInformation.FontColor;
            return font;
        }

        public IFont CreateBoldFont(HSSFWorkbook workbook, FontInformation fontInformation)
        {
            IFont font = CreateFont(workbook, fontInformation);
            font.Boldweight = (short)FontBoldWeight.Bold;
            return font;
        }
    }
}