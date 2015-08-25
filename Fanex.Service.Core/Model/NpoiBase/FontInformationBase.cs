// -----------------------------------------------------------------------
// <copyright file="FontInformationBase.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Model.NpoiBase
{
    using Fanex.Service.Core.Constants;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FontInformationBase
    {
        private string _fontName;

        private short _fontSize;

        public FontInformationBase()
        {
            this._fontName = NPOIConstants.FontName;
            this._fontSize = NPOIConstants.FontSize;
        }

        public FontInformationBase(string fontName, short fontSize)
        {
            this._fontName = fontName;
            this._fontSize = fontSize;
        }

        public string FontName
        {
            get
            {
                return _fontName;
            }
        }

        public short FontSize
        {
            get
            {
                return _fontSize;
            }
        }
    }
}