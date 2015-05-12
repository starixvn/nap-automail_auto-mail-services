// -----------------------------------------------------------------------
// <copyright file="FontInformation.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Model.NpoiOldFormat
{
    using Fanex.Service.Core.Model.NpoiBase;
    using NPOI.HSSF.Util;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FontInformation : FontInformationBase
    {
        private short _fontColor;

        public FontInformation()
            : base()
        {
            this._fontColor = HSSFColor.Black.Index;
        }

        public FontInformation(string fontName, short fontSize, short fontColor)
            : base(fontName, fontSize)
        {
            this._fontColor = fontColor;
        }

        public short FontColor
        {
            get
            {
                return _fontColor;
            }
        }
    }
}