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
        public short LeftBorderStyle;

        public short TopBorderStyle;

        public short RightBorderStyle;

        public short BottomBorderStyle;

        public short LeftBoderColor;

        public short TopBoderColor;

        public short RightBoderColor;

        public short BottomBoderColor;

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
    }

    public struct BorderInfo
    {
        public short LeftBorderStyle;

        public short TopBorderStyle;

        public short RightBorderStyle;

        public short BottomBorderStyle;

        public Color LeftBoderColor;

        public Color TopBoderColor;

        public Color RightBoderColor;

        public Color BottomBoderColor;

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
    }
}