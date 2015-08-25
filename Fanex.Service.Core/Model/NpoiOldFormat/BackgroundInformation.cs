// -----------------------------------------------------------------------
// <copyright file="BackgroundInformation.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Model.NpoiOldFormat
{
    using Fanex.Service.Core.Model.NpoiBase;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BackgroundInformation : BackgroundInformationBase
    {
        private short _fillForegroundColor;

        public BackgroundInformation()
            : base()
        {
            this._fillForegroundColor = HSSFColor.White.Index;
        }

        public BackgroundInformation(int fillPattern, short fillForegroundColor)
            : base(fillPattern)
        {
            this._fillForegroundColor = fillForegroundColor;
        }

        public short FillForegroundColor
        {
            get
            {
                return this._fillForegroundColor;
            }
        }
    }
}