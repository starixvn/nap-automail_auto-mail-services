// -----------------------------------------------------------------------
// <copyright file="CellInfo.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Common
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public struct CellInfo
    {
        public CellInfo(int currentColumn, int currentRow, int endColumn, int endRow, object cellValue)
        {
            this.CurrentColumn = currentColumn;
            this.EndColumn = endColumn;
            this.CurrentRow = currentRow;
            this.EndRow = endRow;
            this.CellValue = cellValue;
        }

        public int CurrentColumn { get; set; }

        public int CurrentRow { get; set; }

        public int EndColumn { get; set; }

        public int EndRow { get; set; }

        public object CellValue { get; set; }
    }
}