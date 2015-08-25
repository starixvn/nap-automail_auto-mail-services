// -----------------------------------------------------------------------
// <copyright file="FontInformation.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Model.NpoiNewFormat
{
    using System.Drawing;
    using Fanex.Service.Core.Model.NpoiBase;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FontInformation : FontInformationBase
    {
        private Color _fontColor;

        public FontInformation()
            : base()
        {
            this._fontColor = Color.Black;
        }

        public FontInformation(string fontName, short fontSize, Color fontColor)
            : base(fontName, fontSize)
        {
            this._fontColor = fontColor;
        }

        public Color FontColor
        {
            get
            {
                return _fontColor;
            }
        }
    }
}