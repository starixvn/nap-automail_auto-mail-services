// -----------------------------------------------------------------------
// <copyright file="BackgroundInfo.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Common
{
    using System.Drawing;

    /// <summary>
    /// Background information.
    /// </summary>
    public struct BackgroundInfo
    {
        public short FillForegroundColor;

        public int FillPattern;

        public BackgroundInfo(
            short fillForegroundColor,
            int fillPattern)
        {
            this.FillForegroundColor = fillForegroundColor;
            this.FillPattern = fillPattern;
        }
    }

    /// <summary>
    /// Background style information.
    /// </summary>
    public struct BackgroundStyleInfo
    {
        public Color FillForegroundColor;

        public int FillPattern;

        public BackgroundStyleInfo(
            Color fillForegroundColor,
            int fillPattern)
        {
            this.FillForegroundColor = fillForegroundColor;
            this.FillPattern = fillPattern;
        }
    }
}