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
        public BackgroundInfo(
            short fillForegroundColor,
            int fillPattern)
        {
            this.FillForegroundColor = fillForegroundColor;
            this.FillPattern = fillPattern;
        }

        public short FillForegroundColor { get; set; }

        public int FillPattern { get; set; }
    }

    /// <summary>
    /// Background style information.
    /// </summary>
    public struct BackgroundStyleInfo
    {
        public BackgroundStyleInfo(
            Color fillForegroundColor,
            int fillPattern)
        {
            this.FillForegroundColor = fillForegroundColor;
            this.FillPattern = fillPattern;            
        }

        public Color FillForegroundColor { get; set; }

        public int FillPattern { get; set; }
    }
}