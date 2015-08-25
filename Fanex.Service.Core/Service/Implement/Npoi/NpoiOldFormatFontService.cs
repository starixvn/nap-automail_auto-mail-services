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
                return CreateFont(new FontInformation());
            }
        }

        public IFont BlackBoldFont
        {
            get
            {
                return CreateBoldFont(new FontInformation());
            }
        }

        public IFont WhiteBoldFont
        {
            get
            {
                FontInformation fontInformation = new FontInformation(NPOIConstants.FontName, NPOIConstants.FontSize, HSSFColor.White.Index);
                return CreateBoldFont(fontInformation);
            }
        }

        public IFont CreateFont(FontInformation fontInformation)
        {
            IFont font = this._workbook.CreateFont();
            font.FontName = fontInformation.FontName;
            font.FontHeightInPoints = fontInformation.FontSize;
            font.Color = fontInformation.FontColor;
            return font;
        }

        public IFont CreateBoldFont(FontInformation fontInformation)
        {
            IFont font = CreateFont(fontInformation);
            font.Boldweight = (short)FontBoldWeight.Bold;
            return font;
        }
    }
}