// -----------------------------------------------------------------------
// <copyright file="BorderInformation.cs" company="Nexcel Solutions Vietnam.">
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
    public class BorderInformation : BorderInformationBase
    {
        private Color _leftBoderColor;

        private Color _topBoderColor;

        private Color _rightBoderColor;

        private Color _bottomBoderColor;

        public BorderInformation()
            : base()
        {
            this._leftBoderColor = Color.Gray;
            this._topBoderColor = Color.Gray;
            this._rightBoderColor = Color.Gray;
            this._bottomBoderColor = Color.Gray;
        }

        public BorderInformation(
            short leftBorderStyle,
            short topBorderStyle,
            short rightBorderStyle,
            short bottomBorderStyle,
            Color leftBoderColor,
            Color topBoderColor,
            Color rightBoderColor,
            Color bottomBoderColor)
            : base(leftBorderStyle, topBorderStyle, rightBorderStyle, bottomBorderStyle)
        {
            this._leftBoderColor = leftBoderColor;
            this._topBoderColor = topBoderColor;
            this._rightBoderColor = rightBoderColor;
            this._bottomBoderColor = bottomBoderColor;
        }

        public Color LeftBoderColor
        {
            get
            {
                return _leftBoderColor;
            }
        }

        public Color TopBoderColor
        {
            get
            {
                return _topBoderColor;
            }
        }

        public Color RightBoderColor
        {
            get
            {
                return _rightBoderColor;
            }
        }

        public Color BottomBoderColor
        {
            get
            {
                return _bottomBoderColor;
            }
        }
    }
}