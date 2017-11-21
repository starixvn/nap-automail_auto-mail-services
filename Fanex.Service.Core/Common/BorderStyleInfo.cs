// -----------------------------------------------------------------------
// <copyright file="BorderStyleInfo.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Common
{
    using System.Drawing;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public struct BorderStyleInfo
    {
        public BorderStyleInfo(
            short leftBorderStyle,
            short topBorderStyle,
            short rightBorderStyle,
            short bottomBorderStyle,
            short leftBoderColor,
            short topBoderColor,
            short rightBoderColor,
            short bottomBoderColor)
        {
            this.LeftBorderStyle = leftBorderStyle;
            this.TopBorderStyle = topBorderStyle;
            this.RightBorderStyle = rightBorderStyle;
            this.BottomBorderStyle = bottomBorderStyle;
            this.LeftBoderColor = leftBoderColor;
            this.TopBoderColor = topBoderColor;
            this.RightBoderColor = rightBoderColor;
            this.BottomBoderColor = bottomBoderColor;
        }

        public short LeftBorderStyle { get; set; }

        public short TopBorderStyle { get; set; }

        public short RightBorderStyle { get; set; }

        public short BottomBorderStyle { get; set; }

        public short LeftBoderColor { get; set; }

        public short TopBoderColor { get; set; }

        public short RightBoderColor { get; set; }

        public short BottomBoderColor { get; set; }
    }

    /// <summary>
    /// Border information.
    /// </summary>
    public struct BorderInfo
    {
        public BorderInfo(
            short leftBorderStyle,
            short topBorderStyle,
            short rightBorderStyle,
            short bottomBorderStyle,
            Color leftBoderColor,
            Color topBoderColor,
            Color rightBoderColor,
            Color bottomBoderColor)
        {
            this.LeftBorderStyle = leftBorderStyle;
            this.TopBorderStyle = topBorderStyle;
            this.RightBorderStyle = rightBorderStyle;
            this.BottomBorderStyle = bottomBorderStyle;
            this.LeftBoderColor = leftBoderColor;
            this.TopBoderColor = topBoderColor;
            this.RightBoderColor = rightBoderColor;
            this.BottomBoderColor = bottomBoderColor;
        }

        public short LeftBorderStyle { get; set; }

        public short TopBorderStyle { get; set; }

        public short RightBorderStyle { get; set; }

        public short BottomBorderStyle { get; set; }

        public Color LeftBoderColor { get; set; }

        public Color TopBoderColor { get; set; }

        public Color RightBoderColor { get; set; }

        public Color BottomBoderColor { get; set; }
    }
}