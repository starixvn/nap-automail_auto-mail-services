// -----------------------------------------------------------------------
// <copyright file="BorderInformationBase.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Model.NpoiBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BorderInformationBase
    {
        private short _leftBorderStyle;

        private short _topBorderStyle;

        private short _rightBorderStyle;

        private short _bottomBorderStyle;

        public BorderInformationBase()
        {
            this._leftBorderStyle = (short)BorderStyle.Thin;
            this._topBorderStyle = (short)BorderStyle.Thin;
            this._rightBorderStyle = (short)BorderStyle.Thin;
            this._bottomBorderStyle = (short)BorderStyle.Thin;
        }

        public BorderInformationBase(short leftBorderStyle, short topBorderStyle, short rightBorderStyle, short bottomBorderStyle)
        {
            this._leftBorderStyle = leftBorderStyle;
            this._topBorderStyle = topBorderStyle;
            this._rightBorderStyle = rightBorderStyle;
            this._bottomBorderStyle = bottomBorderStyle;
        }

        public short LeftBorderStyle
        {
            get
            {
                return _leftBorderStyle;
            }
        }

        public short TopBorderStyle
        {
            get
            {
                return _topBorderStyle;
            }
        }

        public short RightBorderStyle
        {
            get
            {
                return _rightBorderStyle;
            }
        }

        public short BottomBorderStyle
        {
            get
            {
                return _bottomBorderStyle;
            }
        }
    }
}
