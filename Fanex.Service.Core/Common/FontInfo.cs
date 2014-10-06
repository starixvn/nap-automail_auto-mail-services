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
        public string FontName;

        public short FontSize;

        public short FontColor;

        public FontInfo(
            string fontName,
            short fontSize,
            short fontColor)
        {
            this.FontName = fontName;
            this.FontSize = fontSize;
            this.FontColor = fontColor;
        }
    }

    /// <summary>
    /// Font information.
    /// </summary>
    public struct FontAndColorInfo
    {
        public string FontName;

        public short FontSize;

        public Color FontColor;

        public FontAndColorInfo(
            string fontName,
            short fontSize,
            Color fontColor)
        {
            this.FontName = fontName;
            this.FontSize = fontSize;
            this.FontColor = fontColor;
        }
    }
}