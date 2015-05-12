// -----------------------------------------------------------------------
// <copyright file="BackgroundInformationBase.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Model.NpoiBase
{
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BackgroundInformationBase
    {
        private int _fillPattern;

        public BackgroundInformationBase()
        {
            this._fillPattern = (int)FillPattern.SolidForeground;
        }

        public BackgroundInformationBase(int fillPattern)
        {
            this._fillPattern = fillPattern;
        }

        public int FillForegroundPattern
        {
            get
            {
                return this._fillPattern;
            }
        }
    }
}