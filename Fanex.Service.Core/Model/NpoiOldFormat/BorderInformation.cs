// -----------------------------------------------------------------------
// <copyright file="BorderInformation.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Model.NpoiOldFormat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Service.Core.Model.NpoiBase;
    using NPOI.HSSF.Util;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BorderInformation : BorderInformationBase
    {
        private short _leftBoderColor;

        private short _topBoderColor;

        private short _rightBoderColor;

        private short _bottomBoderColor;

        public BorderInformation()
            : base()
        {
            this._leftBoderColor = HSSFColor.Grey80Percent.Index;
            this._topBoderColor = HSSFColor.Grey80Percent.Index;
            this._rightBoderColor = HSSFColor.Grey80Percent.Index;
            this._bottomBoderColor = HSSFColor.Grey80Percent.Index;
        }

        public BorderInformation(
            short leftBorderStyle,
            short topBorderStyle,
            short rightBorderStyle,
            short bottomBorderStyle,
            short leftBoderColor,
            short topBoderColor,
            short rightBoderColor,
            short bottomBoderColor)
            : base(leftBorderStyle, topBorderStyle, rightBorderStyle, bottomBorderStyle)
        {
            this._leftBoderColor = leftBoderColor;
            this._topBoderColor = topBoderColor;
            this._rightBoderColor = rightBoderColor;
            this._bottomBoderColor = bottomBoderColor;
        }

        public short LeftBoderColor
        {
            get
            {
                return _leftBoderColor;
            }
        }

        public short TopBoderColor
        {
            get
            {
                return _topBoderColor;
            }
        }

        public short RightBoderColor
        {
            get
            {
                return _rightBoderColor;
            }
        }

        public short BottomBoderColor
        {
            get
            {
                return _bottomBoderColor;
            }
        }
    }
}
