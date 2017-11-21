// -----------------------------------------------------------------------
// <copyright file="AlignmentInfo.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Common
{
    /// <summary>
    /// Alignment information.
    /// </summary>
    public struct AlignmentInfo
    {
        public AlignmentInfo(
            int horizontalAlign,
            int verticalAlign)
        {
            this.HorizontalAlign = horizontalAlign;
            this.VerticalAlign = verticalAlign;
        }

        public int HorizontalAlign { get; set; }

        public int VerticalAlign { get; set; }
    }
}