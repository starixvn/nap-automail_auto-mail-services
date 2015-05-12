// -----------------------------------------------------------------------
// <copyright file="BackgroundInformation.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Model.NpoiNewFormat
{
    using System.Drawing;
    using Fanex.Service.Core.Model.NpoiBase;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BackgroundInformation : BackgroundInformationBase
    {
        private Color _fillForegroundColor;

        public BackgroundInformation()
            : base()
        {
            this._fillForegroundColor = Color.White;
        }

        public BackgroundInformation(int fillPattern, Color fillForegroundColor)
            : base(fillPattern)
        {
            this._fillForegroundColor = fillForegroundColor;
        }

        public Color FillForegroundColor
        {
            get
            {
                return this._fillForegroundColor;
            }
        }
    }
}