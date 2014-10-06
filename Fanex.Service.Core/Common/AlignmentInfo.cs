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
        public int HorizontalAlign;

        public int VerticalAlign;

        public AlignmentInfo(
            int horizontalAlign,
            int verticalAlign)
        {
            this.HorizontalAlign = horizontalAlign;
            this.VerticalAlign = verticalAlign;
        }
    }
}