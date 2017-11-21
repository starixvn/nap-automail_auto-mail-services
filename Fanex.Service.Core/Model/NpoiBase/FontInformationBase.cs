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
        private readonly string _fontName;

        private readonly short _fontSize;

        public FontInformationBase()
        {
            this._fontName = NpoiConstants.FontName;
            this._fontSize = NpoiConstants.FontSize;
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