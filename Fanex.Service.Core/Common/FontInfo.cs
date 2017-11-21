// -----------------------------------------------------------------------
// <copyright file="FontInfo.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Common
{
    using System.Drawing;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public struct FontInfo
    {
        public FontInfo(
            string fontName,
            short fontSize,
            short fontColor)
        {
            this.FontName = fontName;
            this.FontSize = fontSize;
            this.FontColor = fontColor;
        }

        public string FontName { get; set; }

        public short FontSize { get; set; }

        public short FontColor { get; set; }
    }

    /// <summary>
    /// Font information.
    /// </summary>
    public struct FontAndColorInfo
    {
        public FontAndColorInfo(
            string fontName,
            short fontSize,
            Color fontColor)
        {
            this.FontName = fontName;
            this.FontSize = fontSize;
            this.FontColor = fontColor;
        }

        public string FontName { get; set; }

        public short FontSize { get; set; }

        public Color FontColor { get; set; }
    }
}